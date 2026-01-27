using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player _Player;
    private EnemyController _Controller;

    // TODO EnemySO shit
    private float _Health = 5f;
    public float Health => _Health;

    public void RegisterPlayer(Player player)
    {
        _Player = player;

        _Controller = GetComponent<EnemyController>();
        _Controller.Setup(player);
    }

    public void TakeDamage(float damage, Vector3? attackDirection = null, float knockbackSpeed = 0)
    {
        _Health -= damage;

        if (_Health <= 0)
        {
            Destroy(gameObject);
        }

        if (attackDirection != null) {
            _Controller.Knockback(attackDirection.Value, knockbackSpeed);
        }
    }
}
