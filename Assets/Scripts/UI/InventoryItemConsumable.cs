using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemConsumable : MonoBehaviour
{
    [SerializeField] private Image _Icon;
    [SerializeField] private TextMeshProUGUI _Name;
    [SerializeField] private TextMeshProUGUI _Quantity;
    [SerializeField] private TextMeshProUGUI _Description;
    [SerializeField] private Button _UseButton;

    public void Setup(InventoryItem item, Player player, Action refreshInventory)
    {
        _Icon.sprite = item.ItemSO.Sprite;
        _Name.text = item.ItemSO.Name;
        _Quantity.text = $"x {item.Quantity}";
        _Description.text = item.ItemSO.Description;
        _UseButton.onClick.AddListener(() =>
        {
            foreach (ConsumableEffect effect in item.ItemSO.Effects)
            {
                if (effect is RestoreHealthEffect)
                {
                    try
                    {
                        effect.Use(player, item);
                        item.DecreaseQuantity();
                        refreshInventory();
                    }
                    catch (HealthAlreadyAtMaxException exception)
                    {
                        Debug.Log("Player already at max health");
                        // TODO send event to UI to do something
                    }
                }
            }
        });
    }
}
