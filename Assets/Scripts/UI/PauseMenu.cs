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
}
