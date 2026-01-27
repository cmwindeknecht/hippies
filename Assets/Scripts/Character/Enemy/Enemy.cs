using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _Player;
    private EnemyController _Controller;
    private CharacterStats _Stats;
    private EnemyInventory _Inventory;
    // TODO TEMP --- should be passed in during a setup function or something; basic plan
    //      1. Enemy Prefab
    //      2. SO has sprite / animatinos / etc
    //      3. Database or whatever provides the SO to the spawn manager
    //      4. Spawn manager passes enemySO here
    [SerializeField] private EnemySO _EnemySO;

    public void RegisterPlayer(Player player)
    {
        _Player = player;

        _Controller = GetComponent<EnemyController>();
        _Controller.Setup(player, _EnemySO);

        _Stats = GetComponent<CharacterStats>();
        _Stats.Setup(_EnemySO);

        _Inventory = GetComponent<EnemyInventory>();
        _Inventory.EquipWeapon(_EnemySO.WeaponSO);
    }

    public void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Stats.TakeDamage(damage);

        if (_Stats.CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (attackDirection != null) {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }
}
