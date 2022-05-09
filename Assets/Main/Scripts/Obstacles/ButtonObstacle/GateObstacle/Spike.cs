using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameObject ReturnObstacleParent()
    {
        return transform.parent.parent.gameObject;
    }
}
