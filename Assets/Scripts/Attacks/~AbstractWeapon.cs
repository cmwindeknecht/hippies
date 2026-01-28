using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Weapon
{
    public WeaponSO WeaponSO { get; private set; }
    protected bool CanAttack = true;
    private float _AttackCooldown = 0f;

    public abstract bool Attack();

    // Player
    public virtual void Initialize(WeaponSO weaponSO)
    {
        WeaponSO = weaponSO;
        _AttackCooldown = weaponSO.AttackRate;
    }

    // Enemy --- attackRate from EnemySO
    public virtual void Initialize(WeaponSO weaponSO, float attackRate)
    {
        WeaponSO = weaponSO;
        _AttackCooldown = attackRate;
    }

    protected async UniTaskVoid AttackCooldown()
    {
        // Trigger animation / cooldown here (TODO pass in the animation to the function whenever I figure that shit out)
        await UniTask.WaitForSeconds(_AttackCooldown);
        CanAttack = true;
    }
}