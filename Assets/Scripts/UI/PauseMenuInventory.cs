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

    [Header("Prefabs")]
    [SerializeField] private InventoryItemConsumable _ConsumablePrefab;

    private Player _Player;
    private InventoryItemType _ActiveItemType = InventoryItemType.None; // On click, update this

    public void Setup(Player player)
    {
        _Player = player;
        RefreshInventory();

        _WeaponsButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Weapons;
            RefreshInventory();
        });
        _ArmorButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Armor;
            RefreshInventory();
        });
        _ConsumablesButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Consumable;
            RefreshInventory();
        });
        _OthersButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.Others;
            RefreshInventory();
        });
        _AllButton.onClick.AddListener(() =>
        {
            _ActiveItemType = InventoryItemType.None;
            RefreshInventory();
        });
    }

    private void OnEnable()
    {
        RefreshInventory();
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
                        throw new System.NotImplementedException();
                }
            }
        }
    }
}
