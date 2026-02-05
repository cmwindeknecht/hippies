using UnityEngine;

public enum ArmorSlot
{
    Shield,
    Head,
    Shoulder,
    Hands,
    Torso,
    Legs,
    Feet
}

[CreateAssetMenu(menuName = "ScriptableObjects/Equipment/Armor")]
public class ArmorSO : ItemSO
{
    public ArmorSlot ArmorSlot;
    public float KnockbackResistance = 0f;

    // Damage Type Resistance
    public int PierceResistance = 1;
    public int BluntResistance = 0;
    public int ExplosiveResistance = 0;

    // Magic Resistance
    public int FireResistance = 0;
    public int IceResistance = 0;
    public int LightningResistance = 0;
    public int EarthResistance = 0;
    public int VoidResistance = 0;
}
