using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalObstacle : MonoBehaviour,IObstacle
{
    [SerializeField]
    int damage;
    public void Collide()
    {
        PlayerLevelManager.instance.Remove(damage);
        PlayerLevelUI.RefreshStats?.Invoke();
    }
}
