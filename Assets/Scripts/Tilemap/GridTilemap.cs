using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTilemap : MonoBehaviour
{
    private Tilemap _Tilemap;
    [SerializeField] private TilemapType _TilemapType;
    public TilemapType TilemapType => _TilemapType;
    [SerializeField] private string _Name;
    public string Name => _Name;

    private HashSet<Vector2Int> _Tiles;

    private void Start()
    {
        _Tilemap = GetComponent<Tilemap>();
        PopulateTiles();
        GridTilemapManager.Instance.RegisterTilemap(this);
    }

    private void PopulateTiles()
    {
        _Tiles = new();

        BoundsInt cellBounds = _Tilemap.cellBounds;
        for (int x = cellBounds.xMin; x < cellBounds.xMax; x++)
        {
            for (int y = cellBounds.yMin; y < cellBounds.yMax; y++)
            {
                Vector3Int cellPosition = new(x, y, 0);
                if (_Tilemap.HasTile(cellPosition))
                {
                    _Tiles.Add(new Vector2Int(x, y));
                }
            }
        }
    }

    public bool HasTile(Vector2Int tilePosition)
    {
        if (_Tiles.Count <= 0) throw new System.Exception($"Tilemap {_TilemapType} {_Name} has no tiles!");

        return _Tiles.Contains(tilePosition);
    }

    public Vector3 GetTileCenter(Vector3Int tilePosition)
    {
        return _Tilemap.GetCellCenterWorld(tilePosition);
    }

    public Vector3Int GetTileFromPosition(Vector3 tilePosition)
    {
        return _Tilemap.WorldToCell(tilePosition);
    }
}
