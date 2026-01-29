using UnityEngine;
using UnityEngine.UI;

public class PauseMenuInventory : MonoBehaviour
{
    [Header("Top Level")]
    [SerializeField] private Button _WeaponsButton;
    [SerializeField] private Button _ArmorButton;
    [SerializeField] private Button _ConsumablesButton;
    [SerializeField] private Button _OthersButton;
    [SerializeField] private Button _AllButton;

    [Header("Content")]
    [SerializeField] private RectTransform _ContentRectTransform;
    [SerializeField] private GameObject _ScrollBar; // Hide if content doesn't exceed shit (get height, get count shown * height of each child, show / dont show)

    [Header("Prefabs")]
    [SerializeField] private GameObject _ConsumablePrefab;
}
