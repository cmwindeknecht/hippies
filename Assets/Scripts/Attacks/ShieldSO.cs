using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Shields/Shield")]
public class ShieldSO : ScriptableObject
{
    public float KnockbackResistance = 1f;
    public int DamageNegationMin = 1;
    public int DamageNegationMax = 2;
}
