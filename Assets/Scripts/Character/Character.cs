using System;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Enemy,
    Player
}

public abstract class Character : MonoBehaviour
{
    public class DyanmicStatChangeEvent : EventArgs
    {
        public int Current;
        public int OverTime;
        public int Max;
    }
    public event EventHandler<DyanmicStatChangeEvent> OnHealthChanged;
    public event EventHandler<DyanmicStatChangeEvent> OnEnergyChanged;
    public event EventHandler<DyanmicStatChangeEvent> OnMagicChanged;

    public class OnDynamicStatDepletionEventArgs : EventArgs
    {
        public Character Character;
    }
    public event EventHandler<OnDynamicStatDepletionEventArgs> OnDeath;
    public event EventHandler<OnDynamicStatDepletionEventArgs> OnEnergyDepleted;
    public event EventHandler<OnDynamicStatDepletionEventArgs> OnMagicDepleted;

    protected Rigidbody2D _Rigidbody2D;
    public Vector2 Position => _Rigidbody2D == null ? Vector3.zero : (Vector3)_Rigidbody2D.position;

    protected CharacterStats _Stats;
    public CharacterStats Stats => _Stats;

    protected CharacterInventory _Inventory;
    public Dictionary<InventoryItemType, List<InventoryItem>> InventoryItems => _Inventory.InventoryItems;

    public abstract void TakeDamage(int damage, Character attacker, Vector3? attackDirection = null, float knockbackSpeed = 0);
    public abstract void SpendEnergy(int energy);
    public abstract void SpendMagic(int magic);

    public int GetPossibleDamage(int possibleDamage, DamageType? damageType, List<ElementalDamage> elementalDamageTypes, bool isShielding)
    {
        if (isShielding) possibleDamage -= _Inventory.GetShieldResistance(damageType, elementalDamageTypes);
        return Mathf.Max(possibleDamage - _Inventory.GetArmorResistance(damageType, elementalDamageTypes));
    }

    protected void SendHealthChangeEvent(int overTimeHealth = 0)
    {
        OnHealthChanged?.Invoke(this, new DyanmicStatChangeEvent { Current = _Stats.Health.Current, Max = _Stats.Health.Max, OverTime = overTimeHealth });
    }

    protected void SendEnergyChangeEvent(int overTimeEnergy = 0)
    {
        OnEnergyChanged?.Invoke(this, new DyanmicStatChangeEvent { Current = _Stats.Energy.Current, Max = _Stats.Energy.Max, OverTime = overTimeEnergy });
    }

    protected void SendMagicChangeEvent(int overTimeMagic = 0)
    {
        OnMagicChanged?.Invoke(this, new DyanmicStatChangeEvent { Current = _Stats.Magic.Current, Max = _Stats.Magic.Max, OverTime = overTimeMagic });
    }

    protected void SendDeathEvent()
    {
        OnDeath?.Invoke(this, new OnDynamicStatDepletionEventArgs { Character = this });
    }

    protected void SendEnergyDepletedEvent()
    {
        OnEnergyDepleted?.Invoke(this, new OnDynamicStatDepletionEventArgs { Character = this });
    }

    protected void SendMagicDepletedEvent()
    {
        OnMagicDepleted?.Invoke(this, new OnDynamicStatDepletionEventArgs { Character = this });
    }
}
