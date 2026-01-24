using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _Player;
    private Rigidbody2D _RigidBody;

    // TODO EnemySO shit
    private float health = 5f;
    private float _BaseSpeed = 5f;

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
        
        _RigidBody.linearVelocity = (_Player.gameObject.transform.position - gameObject.transform.position).normalized * GetMovementSpeed();
    }

    public void Setup(Player player)
    {
        _Player = player;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 0)
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
