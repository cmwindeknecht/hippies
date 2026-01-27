using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController _Controller;
    private PlayerInventory _Inventory;

    private float _Health = 5f;
    public float Health => _Health;

    private void Start()
    {
        _Controller = GetComponent<PlayerController>();
        _Inventory = GetComponent<PlayerInventory>();

        GameManager.Instance.RegisterPlayer(this);
    }

    public void TakeDamage(float damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Health -= damage;

        if (_Health <= 0)
        {
            Destroy(gameObject);
        }

        if (attackDirection != null)
        {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }
}
