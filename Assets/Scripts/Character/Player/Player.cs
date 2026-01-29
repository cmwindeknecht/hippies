using System;
using UnityEngine;

public class Player : Character
{
    public class HealthChangedEventArgs : EventArgs
    {
        public int CurrentHealth;
        public int MaxHealth;
    }
    public event EventHandler<HealthChangedEventArgs> OnHealthChanged;
    public event EventHandler OnDeath;

    public CharacterType CharacterType = CharacterType.Player;
    private PlayerController _Controller;
    private PlayerInventory _Inventory;
    private CharacterStats _Stats;

    private void Awake()
    {
        _Controller = GetComponent<PlayerController>();
        _Inventory = GetComponent<PlayerInventory>();
        _Stats = GetComponent<CharacterStats>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

    public void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Stats.TakeDamage(damage);
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs { CurrentHealth = _Stats.CurrentHealth, MaxHealth = _Stats.MaxHealth });

        if (_Stats.CurrentHealth <= 0)
        {
            OnDeath?.Invoke(null, EventArgs.Empty);
            Destroy(gameObject);
        }

        if (attackDirection != null)
        {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }

    public void AddToInventory(ItemSO itemSO)
    {
        _Inventory.AddToInventory(itemSO);
    }
}
