using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
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
    public Dictionary<InventoryItemType, List<InventoryItem>> Inventory => _Inventory.Inventory;
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

    public void RestoreHealth(int health, int iterations, float time)
    {
        if (_Stats.CurrentHealth.Equals(_Stats.MaxHealth))
        {
            throw new HealthAlreadyAtMaxException();
        }

        if (iterations > 1)
        {
            RestoreHealthOverTime(50, 10, 5f, this.GetCancellationTokenOnDestroy()).Forget();
        }
        else
        {
            _Stats.RestoreHealth(health);
            OnHealthChanged?.Invoke(this, new HealthChangedEventArgs { CurrentHealth = _Stats.CurrentHealth, MaxHealth = _Stats.MaxHealth });
        }
    }

    public async UniTaskVoid RestoreHealthOverTime(int totalHealth, int iterations, float totalTime, CancellationToken cancellationToken)
    {
        if (_Stats.CurrentHealth >= _Stats.MaxHealth)
        {
            throw new HealthAlreadyAtMaxException();
        }

        int healthPerTick = totalHealth / iterations;
        int remainder = totalHealth % iterations;
        float delayBetweenTicks = totalTime / iterations;

        for (int i = 0; i < iterations; i++)
        {
            await UniTask.WaitForSeconds(delayBetweenTicks, cancellationToken: cancellationToken);

            // Check if still alive/valid
            if (this == null) return;

            _Stats.RestoreHealth(healthPerTick + (i < remainder ? 1 : 0));
            OnHealthChanged?.Invoke(this, new HealthChangedEventArgs { CurrentHealth = _Stats.CurrentHealth, MaxHealth = _Stats.MaxHealth });

            if (_Stats.CurrentHealth >= _Stats.MaxHealth)
            {
                break;
            }
        }
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
