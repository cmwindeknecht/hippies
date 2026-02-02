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
    public class HealthChangedEventArgs : EventArgs
    {
        public int CurrentHealth;
        public int OverTimeHealth;
        public int MaxHealth;
    }
    public event EventHandler<HealthChangedEventArgs> OnHealthChanged;

    public class OnDeathEventArgs : EventArgs
    {
        public Character DeadCharacter;
    }
    public event EventHandler OnDeath;

    protected Rigidbody2D _Rigidbody2D;
    public Vector2 Position => _Rigidbody2D == null ? Vector3.zero : (Vector3)_Rigidbody2D.position;

    protected CharacterStats _Stats;
    protected CharacterInventory _Inventory;
    public Dictionary<InventoryItemType, List<InventoryItem>> InventoryItems => _Inventory.InventoryItems;

    public abstract void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0);

    public int GetPossibleDamage(int possibleDamage, bool isShielding)
    {
        if (isShielding) possibleDamage -= _Inventory.ShieldResistance;
        return Mathf.Max(possibleDamage - _Inventory.ArmorRating, 0);
    }

    protected void SendHealthChangeEvent(int overTimeHealth = 0)
    {
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs { CurrentHealth = _Stats.Health.Current, MaxHealth = _Stats.Health.Max, OverTimeHealth = overTimeHealth });
    }

    protected void SendOnDeathEvent()
    {
        OnDeath?.Invoke(this, new OnDeathEventArgs { DeadCharacter = this });
    }
}
