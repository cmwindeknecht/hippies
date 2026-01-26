using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileSO _ProjectileSO;
    private Rigidbody2D _Rigidbody;
    private SpriteRenderer _SpriteRenderer;
    private int _WeaponBaseDamage;

    private void Awake()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector3 direction, int weaponBaseDamage)
    {
        _SpriteRenderer.sprite = _ProjectileSO.Sprite;
        _Rigidbody.linearVelocity = direction * _ProjectileSO.Speed;
        _WeaponBaseDamage = weaponBaseDamage;
        Destroy(gameObject, _ProjectileSO.Lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            // TODO way more advanced shit --- need to take into account strength/
            enemy.TakeDamage(_WeaponBaseDamage + Random.Range(_ProjectileSO.DamageMin, _ProjectileSO.DamageMax + 1));
        }

        if (_ProjectileSO.Hollowpoint)
        {
            if (collision.CompareTag("Collision"))
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
