using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get ; private set; }

    public class OnPlayerRegisteredEventArgs : EventArgs
    {
        public Player player;
    }
    public event EventHandler<OnPlayerRegisteredEventArgs> OnPlayerRegistered;

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
        OnPlayerRegistered?.Invoke(this, new OnPlayerRegisteredEventArgs { player = _Player });

        // TODO remove this --- just for testing --- should send an event that player is ready or whatever
        SpawnManager.Instance.SpawnEnemiesForScene(SceneManager.GetActiveScene());
    }
}
