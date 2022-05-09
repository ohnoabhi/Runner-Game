using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenButton : MonoBehaviour
{
    public GameObject ReturnObstacleParent()
    {
        return transform.parent.parent.gameObject;
    }
}
