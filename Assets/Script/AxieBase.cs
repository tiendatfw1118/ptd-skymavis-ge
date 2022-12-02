using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using AxieMixer.Unity;
public class AxieBase : MonoBehaviour
{
    [SerializeField] string axiesID;
    [SerializeField] string axiesGenes;
    void Awake()
    {   
        Mixer.Init();
        var skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        Mixer.SpawnSkeletonAnimation(skeletonAnimation, axiesID, axiesGenes);
    }
}
