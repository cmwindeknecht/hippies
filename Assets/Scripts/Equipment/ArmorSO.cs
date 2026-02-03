using UnityEngine;

public enum ArmorSlot
{
    Head,
    Torso,
    Hands,
    Shoulder,
    Legs,
    Feet
}

[CreateAssetMenu(menuName = "ScriptableObjects/Armor")]
public class ArmorSO : ScriptableObject
{
    public float KnockbackResistance = 1f;
    public int DamageResistance = 1;
    public ArmorSlot ArmorSlot;

    // TODO special resistances like FireDamage or whatever (but needs to be a general thing)
}
