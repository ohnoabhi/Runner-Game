using System;
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
            //UIController.OnGameEnd?.Invoke(true);

            UIScreenController.instance.Show("Win", 0, Array.Empty<object>());
        }
    }


    /*private void LateUpdate()
    {
        if(PlayerLevelManager.instance.GetHealth() <= 0)
        {
            //UIController.OnGameEnd?.Invoke(false);

            UIScreenController.instance.Show("Lose",0,Array.Empty<object>());
        }
    }*/

}
