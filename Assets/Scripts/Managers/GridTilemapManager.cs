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

    private GridTilemap _WalkableTilemap;
    private GridTilemap _CollisionTilemap;

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

    public bool HasTile(Vector2Int position, TilemapType tilemap)
    {
        switch (tilemap)
        {
            case TilemapType.Walkable:
                if (_WalkableTilemap == null) throw new System.Exception("_WalkableTilemap is null!");
                return _WalkableTilemap.HasTile(position) && !_CollisionTilemap.HasTile(position);
            case TilemapType.Collision:
                if (_CollisionTilemap == null) throw new System.Exception("_CollisionTilemap is null!");
                return _CollisionTilemap.HasTile(position) && !_WalkableTilemap.HasTile(position);
            default:
                throw new System.ArgumentException($"Unknown tilemap type: {tilemap}");
        }
    }

    public Vector3 GetTileCenter(Vector3Int position, TilemapType tilemap)
    {
        switch (tilemap)
        {
            case TilemapType.Walkable:
                if (_WalkableTilemap == null) throw new System.Exception("_WalkableTilemap is null!");
                return _WalkableTilemap.GetTileCenter(position);
            case TilemapType.Collision:
                if (_CollisionTilemap == null) throw new System.Exception("_CollisionTilemap is null!");
                return _CollisionTilemap.GetTileCenter(position);
            default:
                throw new System.ArgumentException($"Unknown tilemap type: {tilemap}");
        }
    }

    public Vector3Int GetTileFromPosition(Vector3 position, TilemapType tilemap)
    {
        switch (tilemap)
        {
            case TilemapType.Walkable:
                if (_WalkableTilemap == null) throw new System.Exception("_WalkableTilemap is null!");
                return _WalkableTilemap.GetTileFromPosition(position);
            case TilemapType.Collision:
                if (_CollisionTilemap == null) throw new System.Exception("_CollisionTilemap is null!");
                return _CollisionTilemap.GetTileFromPosition(position);
            default:
                throw new System.ArgumentException($"Unknown tilemap type: {tilemap}");
        }
    }
}
