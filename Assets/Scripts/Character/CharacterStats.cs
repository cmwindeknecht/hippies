using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // TODO temp SerializeField because I don't have a proper character creation screen yet
    [SerializeField] private int _MaxHealth;
    public int MaxHealth => _MaxHealth;
    [SerializeField] private int _CurrentHealth;
    public int CurrentHealth => _CurrentHealth;
    [SerializeField] private float _MovementSpeed;
    public float MovementSpeed => _MovementSpeed;

    // For enemies - populate from SO
    public void Setup(EnemySO enemySO)
    {
        // TODO proper stats that have built in decrease/increase functions and the like for dynamic shit, static stat for shit like strength and the like to get damage mods or whatever, etc
        _MaxHealth = enemySO.Health;
        _CurrentHealth = MaxHealth;
        _MovementSpeed = enemySO.MovementSpeed;
    }

    //public void Initialize(PlayerSaveData saveData)
    //{
    //    MaxHealth = saveData.maxHealth;
    //    CurrentHealth = saveData.currentHealth;
    //    MoveSpeed = saveData.moveSpeed;
    //}

    public void TakeDamage(int damage)
    {
        _CurrentHealth -= damage;
    }

    public void RestoreHealth(int health)
    {
        _CurrentHealth = Mathf.Min(_CurrentHealth + health, _MaxHealth);
    }
}