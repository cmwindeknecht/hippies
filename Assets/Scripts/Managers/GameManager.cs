using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get ; private set; }

    private Player _Player;
    public Player Player => _Player;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterPlayer(Player player)
    {
        if (_Player != null)
        {
            throw new System.Exception("Player attempting to re-register!!");
        }
        _Player = player;

        // TODO remove this --- just for testing --- should send an event that player is ready or whatever
        SpawnManager.Instance.SpawnEnemiesForScene(SceneManager.GetActiveScene());
    }
}
