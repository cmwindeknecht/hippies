using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileSO _ProjectileSO;
    public ProjectileSO ProjectileSO => _ProjectileSO;

    private Rigidbody2D _Rigidbody;
    private int _Damage;
    private Vector3 _AttackDirection;
    private float _AttackTimer;
    private bool _IsInitialized;
    private HashSet<Collider2D> _HitTargets;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _AttackTimer = 0f;
        _IsInitialized = false;
        _HitTargets = new();
    }

    public void Initialize(Vector3 direction, int damage)
    {
        _Damage = damage;
        _AttackDirection = direction.normalized;
        _IsInitialized = true;
        float angle = Mathf.Atan2(_AttackDirection.y, _AttackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Set velocity instead of moving in FixedUpdate
        _Rigidbody.linearVelocity = _AttackDirection * _ProjectileSO.Speed;

        Destroy(gameObject, _ProjectileSO.Lifetime);
    }

    private void Update()
    {
        if (!_IsInitialized) return;
        _AttackTimer += Time.deltaTime;
        if (_AttackTimer >= _ProjectileSO.Lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Bullet hit {collision.gameObject.name} at position {transform.position}");
        Debug.Log($"Contact point: {collision.GetContact(0).point}");

        // Prevent unintended double taps
        if (!_HitTargets.Add(collision.collider))
        {
            return;
        }

        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            Debug.Log($"Ignoring collision with player {player.name}");
            return;
        }

        if (collision.collider.TryGetComponent<Projectile>(out Projectile projectile))
        {
            Debug.Log($"Ignoring collision with projectile {projectile.name}");
            return;
        }

        // TODO need projectile owner so I can use this interchangeably by enemies and players
        if (collision.collider.TryGetComponent<Enemy>(out Enemy enemy))
        {
            // TODO way more advanced shit --- need to take into account strength/
            enemy.TakeDamage(_Damage + Random.Range(_ProjectileSO.DamageMin, _ProjectileSO.DamageMax + 1));
        }

        if (_ProjectileSO.Hollowpoint)
        {
            // Only destroy hollowpoint bullets on collisions
            if (collision.collider.CompareTag("Collision"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}