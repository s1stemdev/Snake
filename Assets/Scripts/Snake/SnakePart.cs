using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    private Vector2Int _cellId;

    public void SetCellPosition(Vector2Int cellId, Vector2 position)
    {
        _cellId = cellId;

        transform.position = position;
    }

    public Vector2Int GetCellId()
    {
        return _cellId;
    }
}
