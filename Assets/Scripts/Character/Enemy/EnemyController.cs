using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _Player;
    private Rigidbody2D _RigidBody;

    private Vector3 _SpawnPosition = Vector3.zero;
    private bool _ChasePlayer = false;
    
    // TODO EnemySO shit
    private float _Health = 5f;
    private float _BaseSpeed = 3f;
    private readonly float _MaxViewDistance = 10f; // I feel like some enemies should see longer than other
    private readonly float _OutOfSightChaseDistance = 5f; // Not sure if I should just do half the max view distance or this should be a const?

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _SpawnPosition = transform.position;
    }

    private void Update()
    {
        if (_Player == null)
        {
            return;
        }
        
        Vector3 playerDirection = (_Player.transform.position - transform.position).normalized;
        LayerMask playerLayerMask = LayerMask.GetMask("Player", "Collision");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, _MaxViewDistance, playerLayerMask);

        Color lineColor = hit && hit.collider.CompareTag("Player") ? Color.green : Color.red;
        Debug.DrawRay(transform.position, playerDirection * _MaxViewDistance, lineColor);

        // If it sees the player / there are no collisions in between, start chasing
        if (hit && hit.collider.CompareTag("Player"))
        {
            _ChasePlayer = true;
        }
        // If the enemy loses sight but they are reasonably close, continue chasing
        else if (_ChasePlayer && Utilities.GetDistanceBetween(_Player.transform.position, transform.position) > _OutOfSightChaseDistance)
        {
            _ChasePlayer = false;
        }

        // Either chase the player or return to the spawn position
        if (_ChasePlayer)
        {
            _RigidBody.linearVelocity = (_Player.transform.position - transform.position).normalized * GetMovementSpeed();
        }
        else if (!Utilities.IsSameVectorPosition(transform.position, _SpawnPosition))
        {
            Debug.Log($"NOT SAME VECTOR POSITION --- spawn {_SpawnPosition} gameobject {transform.position}");
            _ChasePlayer = false;
            _RigidBody.linearVelocity = (_SpawnPosition - transform.position).normalized * GetMovementSpeed();
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
