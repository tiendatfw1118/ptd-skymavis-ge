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
    private bool isClicked;
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

    private void Update()
    {
        healthBar.fillAmount = 1f;
    }
    void OnMouseDown()
    {
        isClicked = !isClicked;
        CalculateGrid();
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
            Debug.Log(GridInitiate.arrayAllocation[currentPosX+1, currentPosY]);
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
}
