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
    [HideInInspector]
    public bool isAttacker;
    public GameController controller;
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
        if (Input.GetMouseButtonDown(0) && controller.isSpawningAxies)
        {
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out x, out y);
            if (x < gridLenght && y < gridHeight && x >= 0 && y >= 0)
            {
                if (arrayAllocation[x, y] == 1)
                {
                    Debug.Log("Already Spawn");
                    return;
                }
                spawnPos = grid.GetWorldPosition(x, y) + new Vector3(10f, 10f) * .5f;
                if (isAttacker)
                {
                    if (controller.IsMaxPower(isAttacker)) return;
                    arrayAllocation[x, y] = 1;
                    controller.currentTotalAttackerPower += axies.GetComponent<AxieBase>().powerPoint;
                    controller.CalculatePower(isAttacker);
                    GameObject attacker = Instantiate(axies, spawnPos, Quaternion.identity);
                    controller.attackers.Add(attacker);
                } 
                else
                {
                    if (controller.IsMaxPower(isAttacker)) return;
                    arrayAllocation[x, y] = 1;
                    controller.currentTotalDefenderPower += axies.GetComponent<AxieBase>().powerPoint;
                    controller.CalculatePower(isAttacker);
                    GameObject defender = Instantiate(axies, spawnPos, Quaternion.identity);
                    controller.defenderers.Add(defender);
                }
            } 
        }
    }
}
