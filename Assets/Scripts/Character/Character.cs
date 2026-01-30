using System;
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

    public abstract void TakeDamage(int damage, Vector3? attackDirection = null, float knockbackSpeed = 0);

    protected void SendHealthChangeEvent(int overTimeHealth = 0)
    {
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs { CurrentHealth = _Stats.CurrentHealth, MaxHealth = _Stats.MaxHealth, OverTimeHealth = overTimeHealth });
    }

    protected void SendOnDeathEvent()
    {
        OnDeath?.Invoke(this, new OnDeathEventArgs { DeadCharacter = this });
    }
}
