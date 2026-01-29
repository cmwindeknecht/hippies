using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image _HealthBarFill;

    private Player _Player;

    private void Start()
    {
        if (GameManager.Instance.Player != null)
        {
            TrackPlayer(GameManager.Instance.Player);
        }
        else
        {
            GameManager.Instance.OnPlayerRegistered += GameManager_OnPlayerRegistered;
        }
    }

    private void GameManager_OnPlayerRegistered(object sender, GameManager.OnPlayerRegisteredEventArgs e)
    {
        if (_Player == null)
        {
            TrackPlayer(e.player);
        }
    }

    private void TrackPlayer(Player player)
    {
        _Player = player;
        _Player.OnHealthChanged += Player_OnHealthChanged;
        _Player.OnDeath += Player_OnDeath;
        _Player.TakeDamage(0); // TODO hacky way to have a current health bar
    }

    private void Player_OnDeath(object sender, System.EventArgs e)
    {
        // TODO show death animation, respawn at the business or whatever makes sense
    }

    private void Player_OnHealthChanged(object sender, Player.HealthChangedEventArgs e)
    {
        UpdateHealth(e.CurrentHealth, e.MaxHealth);
    }

    void UpdateHealth(int currentHealth, int maxHealth)
    {
        _HealthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }
}
