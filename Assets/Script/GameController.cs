using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }
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
    public static bool isStartGame;
    public bool isSpawningAxies;
    public float speedFactor = 1f;
    private float minSpeed = 4f;
    private float maxSpeed = 0.25f;
    public bool spawnAble = true;
    public GameObject attackerWinBanner;
    public GameObject defenderWinBanner;
    private void Awake()
    {
        instance = this;
        grid.axies = attacker;
        Debug.Log(speedFactor / 2);
    }
    public void IncreaseSpeed()
    {
        speedFactor = speedFactor / 2f;
        Debug.Log("Increasing Speed " + speedFactor);
        if (speedFactor <= maxSpeed) speedFactor = maxSpeed;
        foreach (GameObject axie in attackers)
        {
            axie.GetComponent<AxieBase>().speedFactor = speedFactor;
        }
        foreach (GameObject axie in defenderers)
        {
            axie.GetComponent<AxieBase>().speedFactor = speedFactor;
        }
    }

    public void DecreaseSpeed()
    {
        speedFactor = speedFactor * 2f;
        Debug.Log("Decreasing Speed " + speedFactor);
        if (speedFactor >= minSpeed) speedFactor = minSpeed;
        foreach (GameObject axie in attackers)
        {
            axie.GetComponent<AxieBase>().speedFactor = speedFactor;
        }
        foreach (GameObject axie in defenderers)
        {
            axie.GetComponent<AxieBase>().speedFactor = speedFactor;
        }
    }
    public void PauseGame()
    {
        isStartGame = false;
        foreach (GameObject axie in attackers)
        {
            axie.GetComponent<AxieBase>().Pause();
        }
        foreach (GameObject axie in defenderers)
        {
            axie.GetComponent<AxieBase>().Pause();
        }
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
            if (attackerGauge.fillAmount <= 0f) defenderWinBanner.SetActive(true);
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
            if (defenderGauge.fillAmount <= 0f) attackerWinBanner.SetActive(true);
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
        isStartGame = true;
        spawnAble = false;
        if (attackers.Count == 0 || defenderers.Count == 0)
        {
            return;
        }
        else
        {
            foreach (GameObject axie in attackers)
            {
                axie.GetComponent<AxieBase>().StartGame();
            }
            foreach (GameObject axie in defenderers)
            {
                axie.GetComponent<AxieBase>().StartGame();
            }
        }
    }
}
