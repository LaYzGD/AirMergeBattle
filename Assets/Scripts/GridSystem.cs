using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSystem<T>
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector2 _origin;
    private Dictionary<Vector2, T> _positionValuePairs;

    public event Action<Vector2, T> OnValueChanged; 

    public GridSystem(int width, int height, float cellSize, Vector2 origin)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _origin = origin;
    }

    public void CreateGrid(T defaultValue)
    {
        _positionValuePairs = new Dictionary<Vector2, T>();
        
        float currentXPos = _origin.x;
        float currentYPos = _origin.y;
        
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                Vector2 cellCoordinates = new Vector2(currentXPos + (_cellSize / 2), currentYPos);

                _positionValuePairs.Add(cellCoordinates, defaultValue);
                currentXPos += _cellSize;
            }
            currentXPos = _origin.x;
            currentYPos += _cellSize; 
        }   
    }

    public bool TryGetCoordinates(Vector2 posOnGrid, out Vector2 returnValue)
    {
        returnValue = Vector2.zero;

        foreach (var pos in _positionValuePairs.Keys)
        {
            var leftCellSide = pos.x - _cellSize / 2;
            var rightCellSide = pos.x + _cellSize / 2;
            var downCellSide = pos.y - _cellSize / 2;
            var upCellSide = pos.y + _cellSize / 2;
            if (leftCellSide <= posOnGrid.x && posOnGrid.x <= rightCellSide
                && downCellSide <= posOnGrid.y && posOnGrid.y <= upCellSide)
            {
                returnValue = pos;
                return true;
            }
        }

        return false;
    }

    public List<Vector2> GetPositions() 
    {
        return _positionValuePairs.Keys.ToList();
    }

    public void SetValue(Vector2 coordinates, T value) 
    {
        _positionValuePairs[coordinates] = value;
        OnValueChanged?.Invoke(coordinates, value);
    }

    public T GetValue(Vector2 coordinates) 
    {
        return _positionValuePairs.GetValueOrDefault(coordinates);
    }
}
