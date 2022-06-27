using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepPhysics : MonoBehaviour
{
    private void Start()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().Sleep();
    }
}