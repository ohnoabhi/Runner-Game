using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollitionCreature : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = false;

        var rigidbody = collision.transform.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.WakeUp();
        }
    }
}