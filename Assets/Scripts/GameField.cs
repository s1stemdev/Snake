using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{


    public Vector2 CellSize;
    public int CellsInRow = 12;
    

    private Transform _firstCellPosition;
    private Vector2[,] _cellsPositions;
    
    void Start()
    {

        _firstCellPosition = transform.GetChild(0);

    }

    void Update()
    {
        
    }
}
