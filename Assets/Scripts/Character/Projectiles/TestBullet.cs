using UnityEngine;

public class TestBullet : MonoBehaviour
{
    private Rigidbody2D _RigidBody;
    private float maxDistance; // Should be on an SO 

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
        // TODO should check if its an enemy / destroyable / etc and do shit
    }

    public void Fly(Vector2 direction)
    {
        _RigidBody.linearVelocity = direction * 10f;

    }
}
