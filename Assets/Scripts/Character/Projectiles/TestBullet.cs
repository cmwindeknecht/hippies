using UnityEngine;

public class TestBullet : MonoBehaviour
{
    private Rigidbody2D _RigidBody;

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }

        Destroy(gameObject);

        if (collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemy.TakeDamage(3f);
        }
    }

    public void Fly(Vector2 direction)
    {
        _RigidBody.linearVelocity = direction * 10f;

    }
}
