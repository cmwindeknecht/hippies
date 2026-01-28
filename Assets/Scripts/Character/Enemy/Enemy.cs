using UnityEngine;

public class Enemy : Character
{
    public CharacterType CharacterType = CharacterType.Enemy;
    private Player _Player;
    private EnemyController _Controller;
    private CharacterStats _Stats;
    private EnemyInventory _Inventory;
    // TODO TEMP --- should be passed in during a setup function or something; basic plan
    //      1. Enemy Prefab
    //      2. SO has sprite / animatinos / etc
    //      3. Database or whatever provides the SO to the spawn manager
    //      4. Spawn manager passes enemySO here
    public EnemySO EnemySO;
    [SerializeField] GameObject EnemyDropPrefab;

    public void RegisterPlayer(Player player)
    {
        _Player = player;

        _Controller = GetComponent<EnemyController>();
        _Controller.Setup(player, EnemySO);

        _Stats = GetComponent<CharacterStats>();
        _Stats.Setup(EnemySO);

        _Inventory = GetComponent<EnemyInventory>();
        _Inventory.Setup(_Player);
        _Inventory.EquipWeapon(EnemySO.WeaponSO);
    }

    public void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Stats.TakeDamage(damage);

        if (_Stats.CurrentHealth <= 0)
        {
            _Inventory.DropItems();
            Destroy(gameObject);
        }

        if (attackDirection != null) {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }
}
