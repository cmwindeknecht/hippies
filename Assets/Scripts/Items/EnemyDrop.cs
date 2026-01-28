using UnityEngine;

public class EnemyDrop : MonoBehaviour
{   
    private ItemSO _ItemSO;

    private Rigidbody2D _Rigidbody2D;
    private CircleCollider2D _CircleCollider;
    private Player _Player;
    [SerializeField] private float _FlyForce;
    [SerializeField] private float _DistanceToPlayerPickup;
    private bool _IsFlyingAtPlayer = false;

    private void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _CircleCollider = GetComponent<CircleCollider2D>();
    }

    public void Update()
    {
        if (_Player != null && !_IsFlyingAtPlayer)
        {
            Vector2 toPlayerDirection = _Player.Position - _Rigidbody2D.position;
            float distanceToPlayer = toPlayerDirection.magnitude;

            if (distanceToPlayer < _DistanceToPlayerPickup)
            {
                _IsFlyingAtPlayer = true;
                _CircleCollider.isTrigger = true;

                _Rigidbody2D.linearVelocity = (_Player.transform.position - (Vector3) _Rigidbody2D.position).normalized * _FlyForce;
            }
        }
    }

    public void Shoot(Player player, ItemSO itemSO)
    {
        _Player = player;
        _ItemSO = itemSO;

        Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
        _Rigidbody2D.AddForce(randomDir * _FlyForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _Player.AddToInventory(_ItemSO);
            // TODO Play pickup sound
            Destroy(gameObject);
        }
    }
}
