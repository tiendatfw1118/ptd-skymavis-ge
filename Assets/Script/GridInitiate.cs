using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class GridInitiate : MonoBehaviour
{
    public Pathfinding pathfinding;
    public static int gridLength;
    public static int gridHeight;
    public static int[,] arrayAllocation;
    [HideInInspector]
    public GameObject axies;
    [HideInInspector]
    public bool isAttacker;
    public GameController controller;
    public static GridInitiate instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isAttacker = axies.GetComponent<AxieBase>().isAttacker;
        gridLength = 14;
        gridHeight = 10;
        pathfinding = new Pathfinding(gridLength, gridHeight);
        arrayAllocation = new int[gridLength, gridHeight];
    }
    //Todo Click On Grid Create Axies

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && controller.isSpawningAxies)
        {
            SpawnAxies();
        }
    }
    private void SpawnAxies()
    {
        int x;
        int y;
        Vector3 spawnPos = new Vector3();
        Pathfinding.GetGrid().GetXY(UtilsClass.GetMouseWorldPosition(), out x, out y);
        if (x < gridLength -1 && y < gridHeight -1 && x >= 1 && y >= 1)
        {
            if (arrayAllocation[x, y] == 1 || GameController.isStartGame)
            {
                return;
            }
            spawnPos = Pathfinding.GetGrid().GetWorldPosition(x, y) + new Vector3(10f, 10f) * .5f;
            if (isAttacker)
            {
                if (controller.IsMaxPower(isAttacker)) return;
                AxieBase axieBase = axies.GetComponent<AxieBase>();
                arrayAllocation[x, y] = 1;
                controller.currentTotalAttackerPower += axieBase.powerPoint;
                controller.CalculatePower(isAttacker);
                GameObject attacker = Instantiate(axies, spawnPos, Quaternion.identity);
                attacker.GetComponent<AxieBase>().currentPosX = x;
                attacker.GetComponent<AxieBase>().currentPosY = y;
                GameController.instance.attackers.Add(attacker);
            }
            else
            {
                if (controller.IsMaxPower(isAttacker)) return;
                AxieBase axieBase = axies.GetComponent<AxieBase>();
                arrayAllocation[x, y] = 1;
                controller.currentTotalDefenderPower += axieBase.powerPoint;
                controller.CalculatePower(isAttacker);
                GameObject defender = Instantiate(axies, spawnPos, Quaternion.identity);
                defender.GetComponent<AxieBase>().currentPosX = x;
                defender.GetComponent<AxieBase>().currentPosY = y;
                GameController.instance.defenderers.Add(defender);
            }
        }
    }
}
