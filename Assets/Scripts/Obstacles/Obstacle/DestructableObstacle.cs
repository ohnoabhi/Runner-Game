using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] int damage;

    public void Collide(Player player)
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}