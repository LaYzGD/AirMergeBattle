using UnityEngine;

public class GridObject<T>
{
    private GridSystem<GridObject<T>> _grid;
    private float _xPos;
    private float _yPos;
    private T _object;

    public T Object => _object;

    public Vector2 Coordinates => new Vector2(_xPos, _yPos);

    public GridObject(Vector2 position) 
    {
        _xPos = position.x;
        _yPos = position.y;
    }

    public void UpdateCoordinates(Vector2 newCoordinates)
    {
        _xPos = newCoordinates.x;
        _yPos = newCoordinates.y;
    }

    public void SetValue(T obj)
    {
        _object = obj;
    }
}
