using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// TODO should obviously have different drop types and this should be abstract, but just doing this for now for enemy drop / pickup mechanics.
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Item")]
public class ItemSO : ScriptableObject
{
    public InventoryItemType Type;
    public string Name;
    [TextArea] public string Description;
    public Sprite Sprite;
    public List<ConsumableEffect> Effects;

    public int HealthRestored;
    public int HealthIterations = 1; // If it restores health over time, how many iterations
    public int HealthTotalTime = 0; // How long the item takes to finish restoring health
    public int MagicRestored;
    public int MagicIterations = 1; // If it restores mana over time, how many iterations
    public int MagicTotalTime = 0; // How long the item takes to finish restoring magic
}
