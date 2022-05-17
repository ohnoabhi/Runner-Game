using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObstacle : MonoBehaviour, IObstacle
{
    [SerializeField]
    int damage;
   
    public void Collide()
    {
        Player.Instance.TakeDamage(damage);

        //TreeFall(this.gameObject);
    }
 
}
