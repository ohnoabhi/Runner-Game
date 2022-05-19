using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] GameObject logPrefab, trunkPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<ObstacleDetector>() == null) return;
        var treeLogOffset = transform.up * 0.8f;

        var trunkPos = transform.position;

        trunkPos.y = 0.1f;

        Instantiate(logPrefab, transform.position + treeLogOffset,
            Quaternion.Euler(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f)));

        Instantiate(trunkPrefab, trunkPos, transform.rotation);

        Destroy(gameObject);
    }
}