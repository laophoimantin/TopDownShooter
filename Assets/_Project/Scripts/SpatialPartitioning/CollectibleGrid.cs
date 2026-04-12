using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGrid : Singleton<CollectibleGrid>
{
    private Dictionary<Vector2Int, List<CollectibleSP>> _grid = new();
    private float _cellSize = 2f; 

    public void ClearGrid()
    {
        foreach (var cell in _grid.Values)
        {
            cell.Clear();
        }
    }

    public void Register(CollectibleSP item)
    {
        Vector2Int cellCoord = GetCellCoordinate(item.LogicalPosition); 
        
        if (!_grid.ContainsKey(cellCoord))
            _grid[cellCoord] = new List<CollectibleSP>();

        _grid[cellCoord].Add(item);
    }

    public void Unregister(CollectibleSP item)
    {
        Vector2Int cellCoord = GetCellCoordinate(item.LogicalPosition);
        if (_grid.ContainsKey(cellCoord))
        {
            _grid[cellCoord].Remove(item);
        }
    }

    public void GetNearbyItems(Vector3 position, float radius, ref List<CollectibleSP> resultList)
    {
        resultList.Clear();

        Vector2Int centerCell = GetCellCoordinate(position);
        int cellRadius = Mathf.CeilToInt(radius / _cellSize);
        float sqrRadius = radius * radius;

        for (int x = -cellRadius; x <= cellRadius; x++)
        {
            for (int y = -cellRadius; y <= cellRadius; y++)
            {
                Vector2Int cellToCheck = new Vector2Int(centerCell.x + x, centerCell.y + y);
                if (!_grid.TryGetValue(cellToCheck, out var entities)) continue;

                foreach (var mob in entities)
                {
                    Vector2 delta = (Vector2)mob.transform.position - (Vector2)position;
                    if (delta.sqrMagnitude <= sqrRadius)
                        resultList.Add(mob);
                }
            }
        }
    }

    private Vector2Int GetCellCoordinate(Vector3 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x / _cellSize), Mathf.FloorToInt(pos.y / _cellSize));
    }
}