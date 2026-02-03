using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get ; private set; }

    public class OnPlayerRegisteredEventArgs : EventArgs
    {
        public Player player;
    }
    public event EventHandler<OnPlayerRegisteredEventArgs> OnPlayerRegistered;

    public class OnGamePausedEventArgs : EventArgs
    {
        public bool isPaused;
    }
    public event EventHandler<OnGamePausedEventArgs> OnGamePaused;

    private Player _Player;
    public Player Player => _Player;

    private InputAction _PauseInputAction;
    private bool _IsPaused;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _PauseInputAction = InputSystem.actions.FindAction("Pause");
    }

    private void Update()
    {
        if (_PauseInputAction.triggered)
        {
            _IsPaused = !_IsPaused;
        }
        
        HandleIsPaused();
    }

    private void HandleIsPaused()
    {
        Time.timeScale = _IsPaused ? 0.0f : 1.0f;
        OnGamePaused?.Invoke(null, new OnGamePausedEventArgs { isPaused = _IsPaused });
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
