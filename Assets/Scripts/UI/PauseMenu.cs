using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Select Buttons")]
    [SerializeField] private Button _CharacterButton;
    [SerializeField] private Button _SkillsButton;
    [SerializeField] private Button _CharacterInventoryButton;
    [SerializeField] private Button _BusinessInventoryButton;
    [SerializeField] private Button _CompoendiumButton;
    [SerializeField] private Button _OptionsButton;

    [Header("Subsections")]
    [SerializeField] private PauseMenuCharacter _Character;
    [SerializeField] private PauseMenuSkills _Skills;
    [SerializeField] private PauseMenuInventory _Inventory;
    [SerializeField] private PauseMenuBusiness _Business;
    [SerializeField] private PauseMenuCompendium _Compendium;
    [SerializeField] private PauseMenuOptions _Options;

    private Player _Player;

    private void Start()
    {
        if (GameManager.Instance.Player != null)
        {
            _Player = GameManager.Instance.Player;
            TrackPlayer(_Player);
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
            _Player = e.player;
            TrackPlayer(_Player);
        }
    }

    private void TrackPlayer(Player player)
    {
        _Character.Setup(player);
        _Skills.Setup(player);
        _Inventory.Setup(player);
    }
}
