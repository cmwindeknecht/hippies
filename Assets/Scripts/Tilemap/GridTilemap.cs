using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTilemap : MonoBehaviour
{
    [SerializeField] private Tilemap _Tilemap;
    [SerializeField] private TilemapType _TilemapType;
    public TilemapType TilemapType => _TilemapType;
    [SerializeField] private string _Name;
    public string Name => _Name;

    public Dictionary<Vector3Int, bool> _Tiles;

    private void Awake()
    {
        _Tilemap = GetComponent<Tilemap>();
        GridTilemapManager.Instance.RegisterTilemap(this);
        PopulateTiles();
    }

    private void PopulateTiles()
    {
        _Tiles = new Dictionary<Vector3Int, bool>();

        BoundsInt cellBounds = _Tilemap.cellBounds;
        for (int x = cellBounds.xMin; x < cellBounds.xMax; x++)
        {
            for (int y = cellBounds.yMin; y < cellBounds.yMax; y++)
            {
                Vector3Int cellPosition = new(x, y, 0);
                _Tiles.Add(cellPosition, _Tilemap.HasTile(cellPosition));
            }
        }
    }
}
