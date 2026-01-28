using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/Drop")]
public class EnemyDropSO : ScriptableObject
{
    public ItemSO ItemSO;
    public float DropChanceMin = 0f;
    public float DropChanceMax = 1f;
    public int QuantityToDrop = 1;
}
