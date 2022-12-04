using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using AxieMixer.Unity;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class AxieBase : MonoBehaviour
{
    [SerializeField] string axiesID;
    [SerializeField] string axiesGenes;
    [SerializeField] Image healthBar;
    [SerializeField] float hp;
    public GameObject infoPopUp;
    public GameObject hpText;
    public GameObject idText;
    public GameObject damageText;
    private float damage;
    private int currentPathIndex = 0;
    private int currentSelectGridIndex = 0;
    private float maxHP;
    private bool isClicked;
    private List<Vector3> pathVectorList;
    private AxieBase target;
    private bool hasReachedTarget = false;
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
        maxHP = hp;
        healthBar.fillAmount = 1f;
        damage = rand.Next(0, 2);
        Mixer.Init();
        var skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        Mixer.SpawnSkeletonAnimation(skeletonAnimation, axiesID, axiesGenes);
    }
    private void Start()
    {
        if(!isAttacker)
        CalculateGrid();
    }
    public void CalculateHealthBar()
    {
        float newHP = ((hp * 1) / maxHP);
        StartCoroutine(ChangeHealth(healthBar.fillAmount, newHP, 0.6f));
    }

    IEnumerator ChangeHealth(float startHealth, float endHealth, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            healthBar.fillAmount = Mathf.Lerp(startHealth, endHealth, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        healthBar.fillAmount = endHealth;
    }


    void OnMouseDown()
    {
        isClicked = !isClicked;
        infoPopUp.SetActive(isClicked);
        hpText.GetComponent<TextMeshProUGUI>().text = "HP: " + hp + "/" +maxHP;
        idText.GetComponent<TextMeshProUGUI>().text = "ID: " + id;
        damageText.GetComponent<TextMeshProUGUI>().text = "Damage: " + damage;
    }

    //TODO Path Finding + Attack Logi
    void CalculateGrid()
    {
        standableGrid = new List<Vector2>();
        if (IsBelongToGrid(currentPosX + 1, currentPosY) && (GridInitiate.arrayAllocation[currentPosX + 1, currentPosY] != 1)) //Right Grid
        {
            standableGrid.Add(new Vector2(currentPosX + 1, currentPosY));
        }
        if (IsBelongToGrid(currentPosX + 1, currentPosY + 1) && (GridInitiate.arrayAllocation[currentPosX + 1, currentPosY + 1] != 1)) //Top Right Grid
        {
            standableGrid.Add(new Vector2(currentPosX + 1, currentPosY + 1));
        }
        if (IsBelongToGrid(currentPosX + 1, currentPosY - 1) && (GridInitiate.arrayAllocation[currentPosX + 1, currentPosY - 1] != 1)) // Lower Right Grid
        {
            standableGrid.Add(new Vector2(currentPosX + 1, currentPosY - 1));
        }
        if (IsBelongToGrid(currentPosX, currentPosY + 1) && (GridInitiate.arrayAllocation[currentPosX, currentPosY + 1] != 1)) // Top Grid
        {
            standableGrid.Add(new Vector2(currentPosX, currentPosY + 1));
        }
        if (IsBelongToGrid(currentPosX, currentPosY - 1) && (GridInitiate.arrayAllocation[currentPosX, currentPosY - 1] != 1)) // Lower Grid
        {
            standableGrid.Add(new Vector2(currentPosX, currentPosY - 1));
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY) && (GridInitiate.arrayAllocation[currentPosX - 1, currentPosY] != 1)) // Left Grid
        {
            standableGrid.Add(new Vector2(currentPosX - 1, currentPosY));
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY + 1) && (GridInitiate.arrayAllocation[currentPosX - 1, currentPosY + 1] != 1)) //Top Left
        {
            standableGrid.Add(new Vector2(currentPosX - 1, currentPosY + 1));
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY - 1) && (GridInitiate.arrayAllocation[currentPosX - 1, currentPosY - 1] != 1)) //lower left
        {
            standableGrid.Add(new Vector2(currentPosX - 1, currentPosY - 1));
        }
    }

    void FindEnemyGrid()
    {
        Debug.Log(this + " " + id + " is finding target");
        if (IsBelongToGrid(currentPosX + 1, currentPosY) && GetEnemyOnNode(currentPosX + 1, currentPosY)) //Right Grid
        {
            return;
        }
        if (IsBelongToGrid(currentPosX + 1, currentPosY + 1) && GetEnemyOnNode(currentPosX + 1, currentPosY + 1)) //Top Right Grid
        {
            return;
        }
        if (IsBelongToGrid(currentPosX + 1, currentPosY - 1) && GetEnemyOnNode(currentPosX + 1, currentPosY - 1)) // Lower Right Grid
        {
            return;
        }
        if (IsBelongToGrid(currentPosX, currentPosY + 1) && GetEnemyOnNode(currentPosX, currentPosY + 1)) // Top Grid
        {
            return;
        }
        if (IsBelongToGrid(currentPosX, currentPosY - 1) && GetEnemyOnNode(currentPosX, currentPosY - 1)) // Lower Grid
        {
            return;
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY) && GetEnemyOnNode(currentPosX - 1, currentPosY)) // Left Grid
        {
            return;
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY + 1) && GetEnemyOnNode(currentPosX - 1, currentPosY + 1)) //Top Left
        {
            return;
        }
        if (IsBelongToGrid(currentPosX - 1, currentPosY - 1) && GetEnemyOnNode(currentPosX - 1, currentPosY - 1)) //lower left
        {
            return;
        }
    }
    bool IsBelongToGrid(int x, int y)
    {
        return x < GridInitiate.gridLength - 1 && y < GridInitiate.gridHeight - 1 && x >= 1 && y >= 1;
    }
    public void UpdatePos(int x, int y)
    {
        currentPosX = x;
        currentPosY = y;
    }
    public void Action()
    {
        if (isAttacker)
        {
            //TODO loop through Defender list, then loop through standable grid, then do path finding and weight calculate, then move
            if (target == null)
            {
                CalculatePathToEnemy(false);
            }
            if (target != null && !hasReachedTarget)
            {
                HandleMovement(currentPosX, currentPosY);
            }
            if (target != null && hasReachedTarget)
            {
                Attack();
            }
        }
        //Defender Logic
        else 
        {
            if (target == null)
            {
                FindEnemyGrid();
            }
            else
            {
                Attack();
            }
        }
    }

    public void StartGame() 
    {
        InvokeRepeating("Action", 1f, 1f);
    }

    private void CalculatePathToEnemy(bool isRecalculate)
    {
        int numberOfGridTravel = int.MaxValue;
        if (GameController.instance.defenderers.Count == 0) return;
        foreach (GameObject defender in GameController.instance.defenderers)
        {
            var rand = new System.Random();
            AxieBase axie = defender.GetComponent<AxieBase>();
            List<Vector2> attackAblePos = axie.standableGrid;
            currentSelectGridIndex = rand.Next(0, attackAblePos.Count);
            if (!isRecalculate)
            { 
                //Return a random postion to come and attack
                Vector2 targetGrid = attackAblePos[currentSelectGridIndex];

                //Attacker Pos
                Vector3 startPos = Pathfinding.GetGrid().GetWorldPosition(currentPosX, currentPosY);

                //Target Postion
                Vector3 endPos = Pathfinding.GetGrid().GetWorldPosition((int)targetGrid.x, (int)targetGrid.y);

                List<Vector3> allPath = Pathfinding.Instance.FindPath(startPos, endPos);

                //Assign closet target
                if (allPath.Count <= numberOfGridTravel)
                {
                    numberOfGridTravel = allPath.Count;
                    target = defender.GetComponent<AxieBase>();
                    pathVectorList = allPath;
                }
            }
            else
            {
                int newNumber;
                do
                {
                  newNumber = rand.Next(0, attackAblePos.Count);
                } 
                while (currentSelectGridIndex == newNumber);

                currentSelectGridIndex = newNumber;
                Vector2 targetGrid = attackAblePos[currentSelectGridIndex];

                //Attacker Pos
                Vector3 startPos = Pathfinding.GetGrid().GetWorldPosition(currentPosX, currentPosY);

                //Target Postion
                Vector3 endPos = Pathfinding.GetGrid().GetWorldPosition((int)targetGrid.x, (int)targetGrid.y);

                List<Vector3> allPath = Pathfinding.Instance.FindPath(startPos, endPos);

                //Assign closet target
                if (allPath.Count <= numberOfGridTravel)
                {
                    numberOfGridTravel = allPath.Count;
                    target = defender.GetComponent<AxieBase>();
                    pathVectorList = allPath;
                }
            }
        }
    }
    public void HandleMovement(int x, int y)
    {
        //Where axies on grid is occupied
        int previousX = x;
        int previousY = y;
        int currentX;
        int currentY;
        Pathfinding.grid.GetXY(transform.position, out currentX, out currentY);

        //ToDo OverlapTile
        //If Next Tile == 1
        //if(GridInitiate.arrayAllocation[currentX, currentX] == 1)
        //{
            //hasReachedTarget = false;
            //CalculatePathToEnemy(true);
          //  return;
        //}

        currentPosX = currentX;
        currentPosY = currentY;

        GridInitiate.arrayAllocation[previousX, previousY] = 0;
        GridInitiate.arrayAllocation[currentPosX, currentPosY] = 1;

        if (currentPathIndex >= pathVectorList.Count)
        {
            //StopMoving();
            Pathfinding.Instance.GetNode(currentPosX, currentPosY).axiesOnNode.Add(gameObject.GetComponent<AxieBase>());
            hasReachedTarget = true;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, pathVectorList[currentPathIndex] + new Vector3(0,0,-5), 1f);
        if (Vector3.Distance(transform.position, (pathVectorList[currentPathIndex]) + new Vector3(0, 0, -5)) < 1f) currentPathIndex++;
    }
    private void StopMoving()
    {
        pathVectorList = null;
    }

    private bool GetEnemyOnNode(int x, int y)
    {
        List<AxieBase> defenderTarget = Pathfinding.Instance.GetNode(x, y).axiesOnNode;
        if (Pathfinding.Instance.GetNode(x, y).axiesOnNode.Count > 0)
        {
            if (defenderTarget[0].isAttacker) {
                target = defenderTarget[0];
                return true;
            }
            else
            {
                target = null;
                return false;
            }
        } else
        {
            return false;
        }
    }
    private void Attack()
    {
        float targetHP = target.hp;
        if(targetHP <= 0)
        {
            target.Death();
            target = null;
            if(isAttacker)
            hasReachedTarget = false;
        }
        else
        {
            Debug.Log(this+" "+id+ " Start Attacking");
            if((3+ damage - target.damage) % 3 == 0)
            {
                target.hp -= 4;
                target.CalculateHealthBar();
                return;
            }
            if ((3 + damage - target.damage) % 3 == 1)
            {
                target.hp -= 5;
                target.CalculateHealthBar();
                return;
            }
            if ((3 + damage - target.damage) % 3 == 2)
            {
                target.hp -= 3;
                target.CalculateHealthBar();
                return;
            }
            //Todo implement damage logic
        }
    }

    public void Death() 
    {
        Debug.Log(this + " has died");
        if (isAttacker) GameController.instance.attackers.Remove(gameObject);
        else GameController.instance.defenderers.Remove(gameObject);
        Destroy(gameObject);
    }
} 
