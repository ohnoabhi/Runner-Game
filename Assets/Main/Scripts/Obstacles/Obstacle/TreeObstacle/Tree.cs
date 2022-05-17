using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField]
    GameObject logPrefab, trunkPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.other.GetComponent<ObstacleDetector>() != null)
        {

            Debug.Log("OnCollide: " + collision.other.name);
            

            Vector3 treeLogOffset = transform.up * 0.8f;

            Vector3 trunkPos = transform.position;

           

            trunkPos.y = 0.1f;

            Instantiate(logPrefab, transform.position + treeLogOffset, Quaternion.Euler(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f)));

            Instantiate(trunkPrefab, trunkPos, transform.rotation);

            Destroy(this.gameObject);
        }
    }
}
