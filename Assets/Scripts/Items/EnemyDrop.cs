using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{   
    private ItemSO _ItemSO;

    private Rigidbody2D _Rigidbody2D;
    private CircleCollider2D _CircleCollider;
    private Player _Player;
    [SerializeField] private float _FlyForceFromEnemy;
    [SerializeField] private float _FlyForceToPlayer;
    [SerializeField] private float _DistanceToPlayerPickup;
    private bool _IsFlyingAtPlayer = false;

    private const float _ApexScale = .5f;

    private void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _CircleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        if (GameManager.Instance.Player != null)
        {
            _Player = GameManager.Instance.Player;
        }
        else
        {
            GameManager.Instance.OnPlayerRegistered += Instance_OnPlayerRegistered; ;
        }
    }

    private void Instance_OnPlayerRegistered(object sender, GameManager.OnPlayerRegisteredEventArgs e)
    {
        _Player = e.player;
    }

    public void Update()
    {
        Vector2 toPlayerDirection = _Player.Position - _Rigidbody2D.position;
        float distanceToPlayer = toPlayerDirection.magnitude;

        if (_Player != null && !_IsFlyingAtPlayer)
        {
            if (distanceToPlayer < _DistanceToPlayerPickup)
            {
                _IsFlyingAtPlayer = true;
                _CircleCollider.isTrigger = true;
            }
        }

        if (_IsFlyingAtPlayer)
        {
            Vector2 movementSpeed = (_Player.transform.position - (Vector3)_Rigidbody2D.position).normalized * (_FlyForceToPlayer / Mathf.Max(distanceToPlayer, 0.25f));
            if (distanceToPlayer > _DistanceToPlayerPickup)
            {
                _IsFlyingAtPlayer = false;
                movementSpeed = Vector2.zero;
            }
            _Rigidbody2D.linearVelocity = movementSpeed;
        }
    }

    public async UniTaskVoid DropFromEnemy(ItemSO itemSO)
    {
        _ItemSO = itemSO;

        // Apply initial force
        Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        _Rigidbody2D.AddForce(randomDir * _FlyForceFromEnemy, ForceMode2D.Impulse);

        // Animate scale for arc effect
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Scale up then down
            float scaleMultiplier = 1f + Mathf.Sin(t * Mathf.PI) * _ApexScale;
            transform.localScale = startScale * scaleMultiplier;

            await UniTask.Yield();
        }

        transform.localScale = startScale;
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
