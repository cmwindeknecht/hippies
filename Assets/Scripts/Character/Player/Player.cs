using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : Character
{
    public CharacterType CharacterType = CharacterType.Player;
    private PlayerController _Controller;
    private PlayerInventory _Inventory;
    public Dictionary<InventoryItemType, List<InventoryItem>> Inventory => _Inventory.Inventory;

    private int _OverTimeHealth;

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
            _OverTimeHealth = health;
            SendHealthChangeEvent(Mathf.Min(_Stats.CurrentHealth + _OverTimeHealth, _Stats.MaxHealth));
            RestoreHealthOverTime(health, iterations, time, this.GetCancellationTokenOnDestroy()).Forget();
        }
        else
        {
            _Stats.RestoreHealth(health);
            SendHealthChangeEvent();
        }
    }

    public async UniTaskVoid RestoreHealthOverTime(int health, int iterations, float totalTime, CancellationToken cancellationToken)
    {
        if (_Stats.CurrentHealth >= _Stats.MaxHealth)
        {
            throw new HealthAlreadyAtMaxException();
        }

        int healthPerTick = health / iterations;
        int remainder = health % iterations;
        float delayBetweenTicks = totalTime / iterations;

        for (int i = 0; i < iterations; i++)
        {
            await UniTask.WaitForSeconds(delayBetweenTicks, cancellationToken: cancellationToken);

            // Check if still alive/valid
            if (this == null) return;

            _OverTimeHealth -= healthPerTick + (i < remainder ? 1 : 0);
            _Stats.RestoreHealth(healthPerTick + (i < remainder ? 1 : 0));
            SendHealthChangeEvent(Mathf.Min(_Stats.CurrentHealth + _OverTimeHealth, _Stats.MaxHealth));

            if (_Stats.CurrentHealth >= _Stats.MaxHealth)
            {
                _OverTimeHealth = 0;
                break;
            }
        }
    }

    public override void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Stats.TakeDamage(damage);
        SendHealthChangeEvent();

        if (_Stats.CurrentHealth <= 0)
        {
            SendOnDeathEvent();
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
