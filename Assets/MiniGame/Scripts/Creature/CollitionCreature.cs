using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollitionCreature : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collitiion detected");
        GetComponent<Rigidbody>().isKinematic = false;

        if (collision.other.GetComponent<Rigidbody>() != null)
        {
            collision.other.GetComponent<Rigidbody>().WakeUp();
            //collision.other.GetComponent<Rigidbody>().AddExplosionForce(50f,gameObject.transform.forward,30f);
        }

        
       


    }
}
