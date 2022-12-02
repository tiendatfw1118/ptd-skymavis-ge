using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using AxieMixer.Unity;
using TMPro;
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
    public int id;
    public float powerPoint;
    private bool isClicked;
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
        infoPopUp.SetActive(isClicked);
        hpText.GetComponent<TextMeshProUGUI>().text = "HP: "+ hp;
        idText.GetComponent<TextMeshProUGUI>().text = "ID: " +id;
        damageText.GetComponent<TextMeshProUGUI>().text = "Damage: " + damage;
    }

    //TODO Path Finding + Attack Logic
}
