using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player _Player;
    private EnemyController _Controller;

    public void RegisterPlayer(Player player)
    {
        _Player = player;

        _Controller = GetComponent<EnemyController>();
        _Controller.Setup(player);
    }
}
