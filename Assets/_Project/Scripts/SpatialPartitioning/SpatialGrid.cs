using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid : Singleton<SpatialGrid>
{
    [SerializeField] private float _cellSize = 2f; 

    private Dictionary<Vector2Int, List<MobController>> _grid = new();
    private HashSet<Vector2Int> _activeCells = new();
    
    public Vector2Int GetCellCoordinate(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / _cellSize);
        int y = Mathf.FloorToInt(position.y / _cellSize);
        return new Vector2Int(x, y);
    }

   
    public void ClearGrid()
    {
        foreach (var cell in _activeCells)
        {
            if (_grid.TryGetValue(cell, out var list))
                list.Clear();
        }
        _activeCells.Clear();
    }

    public void Register(MobController mob)
    {
        Vector2Int cellCoord = GetCellCoordinate(mob.transform.position);
        if (!_grid.ContainsKey(cellCoord))
            _grid[cellCoord] = new List<MobController>();
    
        _grid[cellCoord].Add(mob);
        _activeCells.Add(cellCoord); 
    }
    
    public void GetNearbyEntities(Vector3 position, float radius, ref List<MobController> resultList)
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
}