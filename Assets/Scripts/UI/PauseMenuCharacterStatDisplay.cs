using TMPro;
using UnityEngine;

public class PauseMenuCharacterStatDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StatValue;

    private Player _Player;

    public void Setup(Player player)
    {
        _Player = player;
    }

    private void OnEnable()
    {
        if (_Player != null)
        {
            StatValue.text = $"{_Player.Stats.Strength.Current}";
        }
    }
}
