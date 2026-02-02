using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : Character
{
    public CharacterType CharacterType = CharacterType.Player;

    private PlayerController _Controller;
    private PlayerVisual _PlayerVisual;
    private new PlayerInventory _Inventory => (PlayerInventory)base._Inventory;

    private int _OverTimeHealth;

    private void Awake()
    {
        _Controller = GetComponent<PlayerController>();
        base._Inventory = GetComponent<PlayerInventory>();
        _Stats = GetComponent<CharacterStats>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        
        _PlayerVisual = GetComponentInChildren<PlayerVisual>();
        _PlayerVisual.Setup(_Controller);
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

    public void RestoreHealth(int health, int iterations, float time)
    {
        if (iterations > 1)
        {
            _OverTimeHealth = health;
            SendHealthChangeEvent(Mathf.Min(_Stats.Health.Current + _OverTimeHealth, _Stats.Health.Max));
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
        if (_Stats.Health.Current >= _Stats.Health.Max)
        {
            throw new DynamicStatAlreadyAtMaxException();
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
            SendHealthChangeEvent(Mathf.Min(_Stats.Health.Current + _OverTimeHealth, _Stats.Health.Max));

            if (_Stats.Health.Current >= _Stats.Health.Max)
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

        if (_Stats.Health.Current <= 0)
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
