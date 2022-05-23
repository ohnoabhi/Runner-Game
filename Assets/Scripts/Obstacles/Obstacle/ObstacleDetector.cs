using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var obstacle = other.GetComponent<IObstacle>();

        if (obstacle != null)
        {
            obstacle.Collide(player);
        }

        else if (other.CompareTag("FinishLine"))
        {
            GameManager.Instance.OnFinish(true);
        }

        else if (other.CompareTag("Pitfall"))
        {
            player.GetComponent<PlayerMovement>().Fall();
        }
    }
}