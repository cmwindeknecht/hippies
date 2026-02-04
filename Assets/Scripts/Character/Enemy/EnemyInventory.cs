using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInventory : CharacterInventory
{
    [SerializeField] private List<EnemyDropSO> _EnemyDropSOs;
    [SerializeField] private EnemyDrop _EnemyDropPrefab; // TODO surely can somehow make this work with the inventory item SOs to just drop that instead, but for now this is fine

    public void DropItems()
    {
        foreach (EnemyDropSO enemyDropSO in _EnemyDropSOs)
        {
            for (int i = 0; i < enemyDropSO.QuantityToDrop; i++)
            {
                if (!Utilities.TestRoll(Random.Range(enemyDropSO.DropChanceMin, enemyDropSO.DropChanceMax)))
                {
                    return;
                }

                EnemyDrop enemyDrop = Instantiate(_EnemyDropPrefab, transform.position, Quaternion.identity);
                enemyDrop.DropFromEnemy(enemyDropSO.ItemSO, enemyDrop.GetCancellationTokenOnDestroy()).Forget();

            }
        }
    }
}
