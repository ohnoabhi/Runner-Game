using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IObstacle obstacle = other.GetComponent<IObstacle>();

        if(obstacle != null)
        {
            obstacle.Collide();
        }

        else if (other.tag == "FinishLine")
        {
            UIController.OnGameEnd?.Invoke(true);
        }
    }
}
