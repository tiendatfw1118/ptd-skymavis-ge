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
    private int currentDefenderTargetX;
    private int currentDefenderTargetY;
    public List<Vector2> standableGrid;
    public bool isDying = false;
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
        if (!isAttacker)
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
        hpText.GetComponent<TextMeshProUGUI>().text = "HP: " + hp + "/" + maxHP;
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
                Debug.Log("Finind");
                CalculatePathToEnemy();
            }
            if (target != null && !hasReachedTarget)
            {
                Debug.Log("Moving");
                HandleMovement(currentPosX, currentPosY);
            }
            if (target != null && hasReachedTarget)
            {
                Debug.Log("Attacking");
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

    private void CalculatePathToEnemy()
    {
        Debug.Log(this + " " + "finding new target");
        int numberOfGridTravel = int.MaxValue;
        if (GameController.instance.defenderers.Count == 0) return;
        foreach (GameObject defender in GameController.instance.defenderers)
        {
            var rand = new System.Random();
            AxieBase axie = defender.GetComponent<AxieBase>();
            List<Vector2> attackAblePos = axie.standableGrid;
            currentSelectGridIndex = rand.Next(0, attackAblePos.Count);
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

        currentPosX = currentX;
        currentPosY = currentY;

        GridInitiate.arrayAllocation[previousX, previousY] = 0;
        GridInitiate.arrayAllocation[currentPosX, currentPosY] = 1;

        if (currentPathIndex >= pathVectorList.Count)
        {
            StopMoving();
            Pathfinding.Instance.GetNode(currentPosX, currentPosY).axiesOnNode.Add(gameObject.GetComponent<AxieBase>());
            hasReachedTarget = true;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, pathVectorList[currentPathIndex] + new Vector3(0, 0, -5), 1f);
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
                currentDefenderTargetX = x;
                currentDefenderTargetY = y;
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
        if (target.hp <= 0 || target.isDying)
        {
            target.Death();
            if(isAttacker)
            {
                currentPathIndex = 0;
                hasReachedTarget = false;
            }
            else
            {
                Pathfinding.Instance.GetNode(currentDefenderTargetX, currentDefenderTargetY).axiesOnNode.Remove(target);
            }
            target = null;
        }
        else
        {
            if (target.isDying) return;
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
        }
    }

    public void Death() 
    {
        if (isDying) return;
        isDying = true;
        if (isAttacker)
        {
            GameController.instance.currentTotalAttackerPower -= powerPoint;
            GameController.instance.attackers.Remove(gameObject);
            GameController.instance.CalculatePower(isAttacker);
        }
        else
        {
            GameController.instance.currentTotalDefenderPower -= powerPoint;
            GameController.instance.defenderers.Remove(gameObject);
            GameController.instance.CalculatePower(isAttacker);
        }
        Destroy(gameObject);
    }
} 
