using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    private Player Player;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var obstacle = other.GetComponent<IObstacle>();

        if (obstacle != null)
        {
            obstacle.Collide(Player);
        }

        else if (other.CompareTag("FinishLine"))
        {
            GameManager.Instance.OnFinish();
        }
    }
}