using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Weapon
{
    //protected Character Owner;  TODO make this so I can use it in the weapon to be like "Owner is enemy, damage player layermask"
    public WeaponSO WeaponSO { get; private set; }
    protected bool CanAttack = true;

    public abstract bool Attack(Vector3 targetPosition);

    public virtual void Initialize(WeaponSO weaponSO)
    {
        WeaponSO = weaponSO;
    }

    protected async UniTaskVoid AttackCooldown()
    {
        // Trigger animation / cooldown here (TODO pass in the animation to the function whenever I figure that shit out)
        CanAttack = false;
        await UniTask.WaitForSeconds(WeaponSO.AttackRate);
        CanAttack = true;
    }
}