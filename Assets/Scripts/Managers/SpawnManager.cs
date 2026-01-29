using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO could probably be a static class, I'll just leave this as is, can reduce later if I want
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    [SerializeField] private List<GameObject> _EnemyPrefabs; // TODO should be stored on the tile or something (especially for shit like popping out of trees)

    private Dictionary<int, Enemy> _Enemies;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _Enemies = new Dictionary<int, Enemy>();
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }


    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        Debug.Log($"SpawnManager: Scene changed from {arg0} to {arg1}");
        ClearEnemies();
        SpawnEnemiesForScene(arg1);
    }

    public void SpawnEnemiesForScene(Scene scene)
    {
        // TODO have a database and what not for spawning enemies, this is just for testing
        for (int i = 0; i < 20; i++) {
            GameObject enemyPrefab = _EnemyPrefabs[Utilities.GetRandomInt(0, _EnemyPrefabs.Count - 1)];
            GameObject instantiatedPrefab = Instantiate(enemyPrefab, new Vector3(i % 2 == 0 ? 6.5f : 5.5f, (i % 2 == 0 ? i + 1 : i * 2) + .5f, 0), Quaternion.identity);
            instantiatedPrefab.name += "-#" + i;
            Enemy enemy = instantiatedPrefab.GetComponent<Enemy>();
            RegisterEnemy(enemy);
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (_Enemies.ContainsKey(enemy.GetInstanceID()))
        {
            throw new System.Exception("Enemy attempting to re-register!");
        }

        enemy.Setup();
        _Enemies.Add(enemy.GetInstanceID(), enemy);
        //Debug.Log($"Registered Enemy {enemy.name}");
    }

    // To be used between biomes
    public void ClearEnemies()
    {
        foreach (Enemy enemy in _Enemies.Values)
        {
            Destroy(enemy.gameObject);
        }
        _Enemies.Clear();
    }
}
