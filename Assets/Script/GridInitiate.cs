using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitiate : MonoBehaviour
{
    void Start()
    {
        Grid grid = new Grid(4, 2, 1f, new Vector3 (0,0,0));
    }

}
