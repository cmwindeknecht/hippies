using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/Consumable")]
public class ItemConsumableSO : ItemSO
{
    public List<ConsumableEffect> ConsumableEffects;
    public int HealthRestored = 0;
    public int HealthIterations = 0; // If it restores health over time, how many iterations
    public int HealthTotalTime = 0; // How long the item takes to finish restoring health
    public int MagicRestored = 0;
    public int MagicIterations = 0; // If it restores mana over time, how many iterations
    public int MagicTotalTime = 0; // How long the item takes to finish restoring magic
    public int EnergyRestored = 0;
    public int EnergyIterations = 0; // If it restores mana over time, how many iterations
    public int EnergyTotalTime = 0; // How long the item takes to finish restoring magic
}
