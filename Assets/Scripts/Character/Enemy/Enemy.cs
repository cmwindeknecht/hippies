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

    public void TakeDamage(float damage)
    {
        _Health -= damage;

        if (_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
