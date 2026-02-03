using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Select Buttons")]
    [SerializeField] private Button _CharacterButton;
    [SerializeField] private Button _SkillsButton;
    [SerializeField] private Button _InventoryButton;
    [SerializeField] private Button _BusinessButton;
    [SerializeField] private Button _CompendiumButton;
    [SerializeField] private Button _OptionsButton;

    [Header("Subsections")]
    [SerializeField] private PauseMenuCharacter _Character;
    [SerializeField] private PauseMenuSkills _Skills;
    [SerializeField] private PauseMenuInventory _Inventory;
    [SerializeField] private PauseMenuBusiness _Business;
    [SerializeField] private PauseMenuCompendium _Compendium;
    [SerializeField] private PauseMenuOptions _Options;

    private Button _LastClicked;

    // TODO hook up these fuckin buttons first

    private Player _Player;

    private void Awake()
    {
        DisableAll();
        _LastClicked = _CharacterButton;
        _LastClicked.onClick.Invoke();

        _CharacterButton.onClick.AddListener(() =>
        {
            DisableAll();
            _Character.gameObject.SetActive(true);
            _LastClicked = _CharacterButton;
        });
        _SkillsButton.onClick.AddListener(() =>
        {
            DisableAll();
            _Skills.gameObject.SetActive(true);
            _LastClicked = _SkillsButton;
        });
        _InventoryButton.onClick.AddListener(() =>
        {
            DisableAll();
            _Inventory.gameObject.SetActive(true);
            _LastClicked = _InventoryButton;
        });
        _BusinessButton.onClick.AddListener(() =>
        {
            DisableAll();
            _Business.gameObject.SetActive(true);
            _LastClicked = _BusinessButton;
        });
        _CompendiumButton.onClick.AddListener(() =>
        {
            DisableAll();
            _Compendium.gameObject.SetActive(true);
            _LastClicked = _CompendiumButton;
        });
        _OptionsButton.onClick.AddListener(() =>
        {
            DisableAll();
            _Options.gameObject.SetActive(true);
            _LastClicked = _OptionsButton;
        });
    }

    private void OnEnable()
    {
        _LastClicked.onClick.Invoke();
    }

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

    private void DisableAll()
    {
        _Character.gameObject.SetActive(false);
        _Skills.gameObject.SetActive(false);
        _Inventory.gameObject.SetActive(false);
        _Business.gameObject.SetActive(false);
        _Compendium.gameObject.SetActive(false);
        _Options.gameObject.SetActive(false);
    }
}
