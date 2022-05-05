using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObstacle : MonoBehaviour,IObstacle
{
    [SerializeField]
    int damage;
    public void Collide()
    {
        PlayerLevelManager.instance.Remove(damage);
        InGameUI.RefreshStats?.Invoke();
        Destroy(gameObject);
    }
}
