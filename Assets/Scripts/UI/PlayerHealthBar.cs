using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO worst case, should just have "PlayerFillBar" to be used with stamina/health/mana --- best case its just a fill bar that works for both enemy and player but thats way trickier
// TODO figure out non filled image version so it doesn't look like shit and all distorted
public class PlayerHealthBar : MonoBehaviour
{
    private Player _Player;
    [SerializeField] private Image _Background;
    [SerializeField] private Image _Outline;
    [SerializeField] private Image _HealthBarFill;
    [SerializeField] private Image _HealthBarFillOverTime;
    [SerializeField] private TextMeshProUGUI _CurrentHealthText;
    [SerializeField] private TextMeshProUGUI _MaxHealthText;

    private int _CurrentHealth = 0;
    private int _MaxHealth = 0;
    private int _OverTimeHealth = 0;

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

    private void Player_OnHealthChanged(object sender, Player.HealthChangedEventArgs e)
    {
        _CurrentHealth = e.CurrentHealth;
        _OverTimeHealth = e.OverTimeHealth;
        _MaxHealth = e.MaxHealth;
        UpdateHealth();
    }

    private void Player_OnDeath(object sender, System.EventArgs e)
    {
        // TODO show death animation, respawn at the business or whatever makes sense
    }

    void UpdateHealth()
    {
        _HealthBarFill.fillAmount = (float) _CurrentHealth / _MaxHealth;
        _HealthBarFillOverTime.fillAmount = (float)_OverTimeHealth / _MaxHealth;

        _CurrentHealthText.text = _OverTimeHealth > _CurrentHealth ? $"{_CurrentHealth} (+{_OverTimeHealth - _CurrentHealth})" : $"{_CurrentHealth}";
        _MaxHealthText.text = $"{_MaxHealth}";
    }
}
