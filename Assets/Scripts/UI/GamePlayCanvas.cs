using UnityEngine;

public class GamePlayCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _HUD;
    [SerializeField] private GameObject _PauseMenu;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += Instance_OnGamePaused;
    }

    private void Instance_OnGamePaused(object sender, GameManager.OnGamePausedEventArgs e)
    {
        if (e.isPaused)
        {
            _HUD.SetActive(false);
            _PauseMenu.SetActive(true);
        }
        else
        {
            _HUD.SetActive(true);
            _PauseMenu.SetActive(false);
        }
    }
}
