using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] GameObject attacker;
    [SerializeField] GameObject defender;
    [SerializeField] GridInitiate grid;
    [SerializeField] int attackerPowerPeak;
    [SerializeField] int defenderPowerPeak;
    [HideInInspector]
    public float currentTotalAttackerPower;
    [HideInInspector]
    public float currentTotalDefenderPower;
    [SerializeField] Image attackerGauge;
    [SerializeField] Image defenderGauge;

    public List<GameObject> attackers;
    public List<GameObject> defenderers;
    public bool isSpawningAxies;
    public GameController instance { get; private set; }
    private void Awake()
    {
        instance = this;
        grid.axies = attacker;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void SelectDefender()
    {
        isSpawningAxies = true;
        grid.axies = defender;
        grid.isAttacker = false;
    }
    public void SelectAttacker()
    {
        isSpawningAxies = true;
        grid.axies = attacker;
        grid.isAttacker = true;
    }
    public float CalculatePower(bool isAttacker)
    {
        if (isAttacker)
        {
            //ToDo calculate total attacker power
            if (IsMaxPower(true))
            {
                attackerGauge.fillAmount = 1f;
                return 1f;
            }
            attackerGauge.fillAmount = (currentTotalAttackerPower * 100) / attackerPowerPeak / 100;
            return attackerGauge.fillAmount;
        }
        else
        {
            if (IsMaxPower(false))
            {
                defenderGauge.fillAmount = 1f;
                return 1f;
            }
            defenderGauge.fillAmount = (currentTotalDefenderPower * 100) / defenderPowerPeak / 100;
            return defenderGauge.fillAmount;
        }
    }
    public bool IsMaxPower(bool isAttacker)
    {
        if (isAttacker)
        {
            return currentTotalAttackerPower >= attackerPowerPeak;
        }
        else return currentTotalDefenderPower >= defenderPowerPeak;
    }
    public void StopSpawning()
    {
        isSpawningAxies = false;
    }

    public void StartGame()
    {
        if (attackers.Count == 0 || defenderers.Count == 0)
        {
            Debug.Log("Can't Start Game");
            return;
        }
        foreach(GameObject attacker in attackers)
        {
            Debug.Log(attacker.GetComponent<AxieBase>().id);
        }
        foreach (GameObject defender in defenderers)
        {
            Debug.Log(defender.GetComponent<AxieBase>().id);
        }
    }
}
