using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject _DynamicStats;

    [Header("Prefabs")]
    [SerializeField] private InventoryItemConsumable _ConsumablePrefab;

    private Player _Player;
    private InventoryItemType _ActiveItemType = InventoryItemType.None;
    private Button _LastClicked;

    public void Setup(Player player)
    {
        _Player = player;
        _LastClicked = _AllButton;
        _LastClicked.onClick.Invoke();

        _WeaponsButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Weapons;
            RefreshInventory();
            _DynamicStats.SetActive(false);
            _LastClicked = _WeaponsButton;
        });
        _ArmorButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Armor;
            RefreshInventory();
            _DynamicStats.SetActive(false);
            _LastClicked = _ArmorButton;
        });
        _ConsumablesButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Consumable;
            RefreshInventory();
            _DynamicStats.SetActive(true);
            _LastClicked = _ConsumablesButton;
        });
        _OthersButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Miscellaneous;
            RefreshInventory();
            _DynamicStats.SetActive(false);
            _LastClicked = _OthersButton;
        });
        _AllButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.None;
            RefreshInventory();
            _DynamicStats.SetActive(true);
            _LastClicked = _AllButton;
        });
    }

    private void OnEnable()
    {
        _LastClicked.onClick.Invoke();
    }

    public void RefreshInventory()
    {
        if (_Player == null) return;

        // Clear existing items
        foreach (Transform child in _ContentRectTransform)
        {
            Destroy(child.gameObject);
        }

        Dictionary<InventoryItemType, List<InventoryItem>> inventory = _Player.InventoryItems;
        foreach (KeyValuePair<InventoryItemType, List<InventoryItem>> kvp in inventory)
        {
            if (!_ActiveItemType.Equals(InventoryItemType.None) && !kvp.Key.Equals(_ActiveItemType)) continue;

            List<InventoryItem> sortedItems = kvp.Value.OrderBy(p => p.ItemSO.Name).ToList();

            foreach (InventoryItem item in sortedItems)
            {
                switch (kvp.Key)
                {
                    case InventoryItemType.Consumable:
                        InventoryItemConsumable consumable = Instantiate(_ConsumablePrefab, _ContentRectTransform);
                        consumable.Setup(item, _Player, RefreshInventory);
                        break;
                    default:
                        Debug.Log($"No Prefab for item type {kvp.Key}");
                        break;
                }
            }
        }
    }
}
