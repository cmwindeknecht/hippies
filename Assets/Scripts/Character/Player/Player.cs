using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController _Controller;
    private PlayerInventory _Inventory;
    private CharacterStats _CharacterStats;

    private void Start()
    {
        _Controller = GetComponent<PlayerController>();
        _Inventory = GetComponent<PlayerInventory>();
        _CharacterStats = GetComponent<CharacterStats>();

        GameManager.Instance.RegisterPlayer(this);
    }

    public void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _CharacterStats.TakeDamage(damage);

        if (_CharacterStats.CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (attackDirection != null)
        {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }
}
