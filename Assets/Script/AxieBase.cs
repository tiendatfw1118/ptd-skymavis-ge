using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using AxieMixer.Unity;
using TMPro;
using System.Collections.Generic;

public class AxieBase : MonoBehaviour
{
    [SerializeField] string axiesID;
    [SerializeField] string axiesGenes;
    [SerializeField] Image healthBar;
    [SerializeField] int hp;
    public GameObject infoPopUp;
    public GameObject hpText;
    public GameObject idText;
    public GameObject damageText;
    private int damage;
    private int currentPathIndex;
    private bool isClicked;
    private List<Vector3> pathVectorList;
    public int id;
    public float powerPoint;
    public bool isAttacker = false;
    public int currentPosX = 0;
    public int currentPosY = 0;
    public List<Vector2> standableGrid;
    void Awake()
    {
        isClicked = false;
        var rand = new System.Random();
        id = rand.Next(1, 999999);
        damage = rand.Next(0, 2);
        Mixer.Init();
        var skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        Mixer.SpawnSkeletonAnimation(skeletonAnimation, axiesID, axiesGenes);
    }
    private void Start()
    {
        CalculateGrid();
    }

    private void Update()
    {
        healthBar.fillAmount = 1f;
    }
    void OnMouseDown()
    {
        isClicked = !isClicked;
        DisPlayPos();
        infoPopUp.SetActive(isClicked);
        hpText.GetComponent<TextMeshProUGUI>().text = "HP: "+ hp;
        idText.GetComponent<TextMeshProUGUI>().text = "ID: " +id;
        damageText.GetComponent<TextMeshProUGUI>().text = "Damage: " + damage;
    }

    //TODO Path Finding + Attack Logic

    void DisPlayPos()
    {
        foreach(Vector2 pos in standableGrid)
            Debug.Log("Stand able grid is: "+pos);
    }
    void CalculateGrid()
    {
        standableGrid = new List<Vector2>();
        if (IsBelongToGrid(currentPosX + 1, currentPosY) && (GridInitiate.arrayAllocation[currentPosX+1, currentPosY] != 1)) //Right Grid
        {
            standableGrid.Add(new Vector2(currentPosX + 1, currentPosY));
        }
        if (IsBelongToGrid(currentPosX + 1, currentPosY + 1) && (GridInitiate.arrayAllocation[currentPosX + 1, currentPosY + 1] != 1)) //Top Right Grid
        {
            standableGrid.Add(new Vector2(currentPosX + 1, currentPosY +1));
        }
        if (IsBelongToGrid(currentPosX + 1, currentPosY - 1) && (GridInitiate.arrayAllocation[currentPosX + 1, currentPosY - 1] != 1)) // Lower Right Grid
        {
            standableGrid.Add(new Vector2(currentPosX + 1, currentPosY - 1));
        }
        if (IsBelongToGrid(currentPosX, currentPosY + 1) && (GridInitiate.arrayAllocation[currentPosX, currentPosY + 1] != 1)) // Top Grid
        {
            standableGrid.Add(new Vector2(currentPosX, currentPosY + 1));
        }
        if(IsBelongToGrid(currentPosX, currentPosY - 1) && (GridInitiate.arrayAllocation[currentPosX, currentPosY - 1] != 1)) // Lower Grid
        {
            standableGrid.Add(new Vector2(currentPosX, currentPosY - 1));
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY) && (GridInitiate.arrayAllocation[currentPosX-1, currentPosY] != 1)) // Left Grid
        {
            standableGrid.Add(new Vector2(currentPosX-1, currentPosY));
        }
        if(IsBelongToGrid(currentPosX - 1, currentPosY + 1) && (GridInitiate.arrayAllocation[currentPosX - 1, currentPosY +1] != 1)) //Top Left
        {
            standableGrid.Add(new Vector2(currentPosX - 1, currentPosY + 1));
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY - 1) && (GridInitiate.arrayAllocation[currentPosX - 1, currentPosY - 1] != 1)) //lower left
        {
            standableGrid.Add(new Vector2(currentPosX - 1, currentPosY - 1));
        }
    }
    bool IsBelongToGrid(int x, int y)
    {
        return x < GridInitiate.gridLength && y < GridInitiate.gridHeight && x >= 0 && y >= 0;
    }
    public void UpdatePos(int x, int y)
    {
        currentPosX = x;
        currentPosY = y;
    }
    public void Action()
    {
        if(isAttacker)
        {
            //TODO loop through Defender list, then loop through standable grid, then do path finding and weight calculate, then move
            Debug.Log(id +" Is Finding Target to kill");
            pathVectorList = CalculatePathToEnemy();
            foreach(var path in pathVectorList)
            {
                Debug.Log(path);
            }
            HandleMovement();
        }
    }

    private List<Vector3> CalculatePathToEnemy()
    {
        GameObject currentTarget;
        List<Vector3> pathToTarget = new List<Vector3>();
        int numberOfGridTravel = int.MaxValue;
        if (GameController.instance.defenderers.Count == 0) return null;
        foreach (GameObject defender in GameController.instance.defenderers)
        {
            var rand = new System.Random();
            AxieBase axie = defender.GetComponent<AxieBase>();
            List<Vector2> attackAblePos = axie.standableGrid;

            //Return a random postion to come and attack
            Vector2 targetGrid = attackAblePos[rand.Next(0, attackAblePos.Count)];

            //Attacker Pos
            Vector3 startPos = Pathfinding.GetGrid().GetWorldPosition(currentPosX, currentPosY);

            //Target Postion
            Vector3 endPos = Pathfinding.GetGrid().GetWorldPosition((int)targetGrid.x, (int)targetGrid.y);

            List<Vector3> allPath = Pathfinding.Instance.FindPath(startPos, endPos);

            //Assign closet target
            if (allPath.Count <= numberOfGridTravel)
            {
                numberOfGridTravel = allPath.Count;
                currentTarget = defender;
                pathToTarget = allPath;
            }
        }
        return pathToTarget;
    }

    public void HandleMovement()
    {
        int i = 0;
        transform.position = Vector3.Lerp(transform.position, pathVectorList[i], 1f);
        while (i < pathVectorList.Count)
        {
            Debug.Log(i);
            transform.position = Vector3.Lerp(transform.position, pathVectorList[i] + new Vector3(0,0,-5), 5f);
            if (Vector3.Distance(transform.position, (pathVectorList[i]) + new Vector3(0, 0, -5)) < 1f) i++;
            if (i == 100) break;
        }

    }
    private void StopMoving()
    {
        pathVectorList = null;
    }
} 
