using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageGaurds : MonoBehaviour
{
    public bool isRunning;
    public Transform finalSoliderPos;

    [SerializeField]
    float speed;

    private void Start()
    {
        finalSoliderPos = transform.parent.parent.GetComponent<CreatureItem>().soliderFinalPos;
    }

    private void Update()
    {
        if (!isRunning) return;

        GetComponent<Animator>().SetBool("Run", true);

        if (transform.position != finalSoliderPos.position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, finalSoliderPos.position, speed * Time.deltaTime);
            this.transform.LookAt(finalSoliderPos);
            GetComponent<Rigidbody>().MovePosition(pos);
        }

        /*else *//*if (transform.position == finalSoliderPos.position)*//*
        {
            Destroy(this.gameObject);
            isRunning = false;
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.name == "FinalSoliderPos") 
        Destroy(this.gameObject);
    }
}
