using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO worst case, should just have "PlayerFillBar" to be used with stamina/health/mana --- best case its just a fill bar that works for both enemy and player but thats way trickier
// TODO figure out non filled image version so it doesn't look like shit and all distorted
public abstract class BarFill : MonoBehaviour
{
    protected Player _Player;
    [SerializeField] private Image _Background;
    [SerializeField] private Image _Outline;
    [SerializeField] private Image _BarFill;
    [SerializeField] private Image _BarFillOverTime;
    [SerializeField] private TextMeshProUGUI _CurrentText;
    [SerializeField] private TextMeshProUGUI _MaxText;

    protected int _Current = 0;
    protected int _Max = 0;
    protected int _OverTime = 0;

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

    protected abstract void TrackPlayer(Player player);

    protected void UpdateBar(int current, int max, int overTime)
    {
        _Current = current;
        _Max = max;
        _OverTime = overTime;

        _BarFill.fillAmount = (float) _Current / _Max;
        _BarFillOverTime.fillAmount = (float)_OverTime / _Max;

        _CurrentText.text = _OverTime > _Current ? $"{_Current} (+{_OverTime - _Current})" : $"{_Current}";
        _MaxText.text = $"{_Max}";
    }
}
