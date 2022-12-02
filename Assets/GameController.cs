using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] AxieBase attacker;
    [SerializeField] AxieBase defender;
    [SerializeField] GridInitiate grid;
    private void Awake()
    {
        grid.axies = attacker;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectDefender() 
    {
        Debug.Log("Spawn Defender");
        grid.axies = defender;
    }
    public void SelectAttacker() 
    {
        Debug.Log("Spawn Attacker");
        grid.axies = attacker;    
    }
}
