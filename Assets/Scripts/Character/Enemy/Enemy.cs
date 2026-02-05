using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Enemy : Character
{
    public CharacterType CharacterType = CharacterType.Enemy;
    private new EnemyInventory _Inventory => (EnemyInventory) base._Inventory;

    private Player _Player;
    private EnemyController _Controller;
    // TODO TEMP --- should be passed in during a setup function or something; basic plan
    //      1. Enemy Prefab
    //      2. SO has sprite / animatinos / etc
    //      3. Database or whatever provides the SO to the spawn manager
    //      4. Spawn manager passes enemySO here
    public EnemySO EnemySO;

    private float _OnDamageYellRadius = 3f;

    private void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
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

    public void Setup()
    {
        _Controller = GetComponent<EnemyController>();
        _Controller.Setup(this, EnemySO);

        _Stats = GetComponent<CharacterStats>();
        _Stats.Setup(EnemySO);

        base._Inventory = GetComponent<EnemyInventory>();
        base._Inventory.EquipWeapon(EnemySO.WeaponSO);

        EnemyWorldCanvas enemyCanvas = GetComponentInChildren<EnemyWorldCanvas>();
        enemyCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        enemyCanvas.RegisterEnemy(this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _OnDamageYellRadius);
    }

    private void NotifyNearbyEnemies()
    {
        LayerMask enemyLayerMask = LayerMask.GetMask(Constants.ENEMY_LAYER);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _OnDamageYellRadius, enemyLayerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                enemy.NotifiedToChasePlayer().Forget();
            }
        }
    }

    public override void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        // TODO need to hook this up properly
        _Stats.TakeDamage(damage);
        SendHealthChangeEvent();

        // TODO due to the hack for the UI
        if (_Stats.Health.Current < _Stats.Health.Max)
        {
            NotifyNearbyEnemies();
            _Controller.NotifiedToChasePlayer().Forget(); // Notify self to chase enemy
        }

        if (_Stats.Health.Current <= 0)
        {
            SendDeathEvent();
            _Inventory.DropItems();
            Destroy(gameObject);
        }

        if (attackDirection != null)
        {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }

    public override void SpendEnergy(int energy)
    {
        throw new NotImplementedException();
    }

    public override void SpendMagic(int magic)
    {
        throw new NotImplementedException();
    }
}
