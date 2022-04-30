using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalObstacle : MonoBehaviour,IObstacle
{
    [SerializeField]
    int damage;
    public void Collide()
    {
        PlayerLevelManager.instance.Minus(damage);
        PlayerLevelUI.RefreshStats?.Invoke();
    }
}
