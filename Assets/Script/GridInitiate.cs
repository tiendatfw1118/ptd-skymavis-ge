using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class GridInitiate : MonoBehaviour
{
    private Grid grid;
    private int gridLenght;
    private int gridHeight;
    private int[,] arrayAllocation;
    [HideInInspector]
    public GameObject axies;
    void Start()
    {
        gridLenght = 14;
        gridHeight = 10;
        grid = new Grid(gridLenght, gridHeight, 10f, new Vector3 (-70,-50,0));
        arrayAllocation = new int[gridLenght, gridHeight];
    }
    //Todo Click On Grid Create Axies

    private void Update()
    {
        int x;
        int y;
        Vector3 spawnPos = new Vector3();
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out x, out y);
            if (x < gridLenght && y < gridHeight && x >= 0 && y >= 0)
            {
                if (arrayAllocation[x, y] == 1)
                {
                    return;
                }
                arrayAllocation[x, y] = 1;
                spawnPos = grid.GetWorldPosition(x, y) + new Vector3(10f, 10f) * .5f;
                Debug.Log(grid.GetWorldPosition(x, y));
                Instantiate(axies, spawnPos, Quaternion.identity);
                
            }
                
        }
    }
}
