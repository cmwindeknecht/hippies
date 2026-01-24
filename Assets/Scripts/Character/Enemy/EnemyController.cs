using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _Player;
    private Rigidbody2D _RigidBody;

    // TODO EnemySO shit
    private float _Health = 5f;
    private float _BaseSpeed = 5f;
    private float _ViewDistance = 10f; 

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_Player == null)
        {
            return;
        }

        Vector3 playerDirection = (_Player.transform.position - transform.position).normalized;
        LayerMask playerLayerMask = LayerMask.GetMask("Player", "Collision");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, _ViewDistance, playerLayerMask);

        Color lineColor = hit && hit.collider.CompareTag("Player") ? Color.green : Color.red;
        Debug.DrawRay(transform.position, playerDirection * _ViewDistance, lineColor);

        if (hit && hit.collider.CompareTag("Player"))
        {
            _RigidBody.linearVelocity = (_Player.gameObject.transform.position - gameObject.transform.position).normalized * GetMovementSpeed();
        }
    }

    public void Setup(Player player)
    {
        _Player = player;
    }

    public void TakeDamage(float damage)
    {
        _Health -= damage;

        if (_Health < 0)
        {
            Destroy(gameObject);
        }
    }

    private float GetMovementSpeed()
    {
        // TODO use input to determine walk/sprint/sneak/etc
        return _BaseSpeed;
    }
}
