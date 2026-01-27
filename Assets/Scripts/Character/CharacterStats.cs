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
    public void Initialize(EnemySO enemySO)
    {
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
}