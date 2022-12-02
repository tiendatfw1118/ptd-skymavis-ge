using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using AxieMixer.Unity;
public class AxieBase : MonoBehaviour
{
    [SerializeField] string axiesID;
    [SerializeField] string axiesGenes;
    [SerializeField] Image healthBar;
    void Awake()
    {   
        Mixer.Init();
        var skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        Mixer.SpawnSkeletonAnimation(skeletonAnimation, axiesID, axiesGenes);
    }

    private void Update()
    {
        healthBar.fillAmount = 0.5f;
    }
}
