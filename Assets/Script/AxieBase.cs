using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using AxieMixer.Unity;
public class AxieBase : MonoBehaviour
{
    [SerializeField] string axiesID;
    [SerializeField] string axiesGenes;

    // Start is called before the first frame update
    void Awake()
    {   
        Mixer.Init();
        var skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        Mixer.SpawnSkeletonAnimation(skeletonAnimation, axiesID, axiesGenes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
