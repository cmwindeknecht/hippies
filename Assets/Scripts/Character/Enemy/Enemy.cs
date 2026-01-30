using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Enemy : Character
{
    public CharacterType CharacterType = CharacterType.Enemy;
    private Player _Player;
    private EnemyController _Controller;
    private EnemyInventory _Inventory;
    // TODO TEMP --- should be passed in during a setup function or something; basic plan
    //      1. Enemy Prefab
    //      2. SO has sprite / animatinos / etc
    //      3. Database or whatever provides the SO to the spawn manager
    //      4. Spawn manager passes enemySO here
    public EnemySO EnemySO;
    [SerializeField] GameObject EnemyDropPrefab;

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
        _Controller.Setup(EnemySO);

        _Stats = GetComponent<CharacterStats>();
        _Stats.Setup(EnemySO);

        _Inventory = GetComponent<EnemyInventory>();
        _Inventory.EquipWeapon(EnemySO.WeaponSO);

        EnemyWorldCanvas enemyCanvas = GetComponentInChildren<EnemyWorldCanvas>();
        enemyCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        enemyCanvas.RegisterEnemy(this);
    }

    public override void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Stats.TakeDamage(damage);
        SendHealthChangeEvent();

        if (_Stats.CurrentHealth <= 0)
        {
            SendOnDeathEvent();
            _Inventory.DropItems();
            Destroy(gameObject);
        }

        if (attackDirection != null) {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }
}
