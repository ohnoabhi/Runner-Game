using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IObstacle
{
    [SerializeField] GameObject logPrefab, trunkPrefab;
    [SerializeField] private float damage = 5;

    public void Collide(Player player)
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
        var transformPosition = transform.position;
        Instantiate(logPrefab, transformPosition,
            Quaternion.Euler(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f)));
        Instantiate(trunkPrefab, transformPosition, transform.rotation);
        Destroy(gameObject);
    }
}