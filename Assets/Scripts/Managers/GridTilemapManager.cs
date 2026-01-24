using UnityEngine;
using UnityEngine.Tilemaps;

public enum TilemapType
{
    Walkable,
    Collision
}

public class GridTilemapManager : MonoBehaviour
{
    public static GridTilemapManager Instance { get; private set; }

    [SerializeField] private GridTilemap _WalkableTilemap;
    [SerializeField] private GridTilemap _CollisionTilemap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterTilemap(GridTilemap tilemap)
    {
        
        switch (tilemap.TilemapType)
        {
            case TilemapType.Walkable:
                Debug.Log($"Setting {tilemap.TilemapType} from {(_WalkableTilemap == null ? "UNSET" : _WalkableTilemap.Name)} to {tilemap.Name}");
                _WalkableTilemap = tilemap; 
                break;
            case TilemapType.Collision:
                Debug.Log($"Setting {tilemap.TilemapType} from {(_CollisionTilemap == null ? "UNSET" : _CollisionTilemap.Name)} to {tilemap.Name}");
                _CollisionTilemap = tilemap; 
                break;
        }
    }
}
