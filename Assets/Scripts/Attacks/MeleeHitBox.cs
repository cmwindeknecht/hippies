using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public float Radius = 1f;
    public float Duration = 0.5f;
    private Vector3 center;
    private Vector3 startDir;
    private float timer = 0f;
    private bool initialized = false;

    public void Initialize(Vector3 playerPosition, Vector3 forwardDir, float reach, float attackDuration)
    {
        center = playerPosition;
        startDir = forwardDir.normalized;
        Radius = reach;
        Duration = attackDuration;
        timer = 0f;
        transform.position = center + startDir * Radius;
        initialized = true;        
    }

    private void Update()
    {
        if (!initialized) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / Duration);

        // Sweep 180 degrees, left to right
        float angleOffset = Mathf.Lerp(90f, -90f, t);

        // rotate in 2D correctly
        float baseAngle = Mathf.Atan2(startDir.y, startDir.x) * Mathf.Rad2Deg;
        float finalAngle = baseAngle + angleOffset;

        // convert angle back to direction
        float rad = finalAngle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);

        transform.position = center + dir * Radius;

        if (t >= 1f)
            Destroy(gameObject);
    }
}
