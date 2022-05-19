using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObstacleDetector : MonoBehaviour
{
    public Player Player;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GreenButton>() != null)
        {
            var parent = other.GetComponent<GreenButton>().ReturnObstacleParent();

            if(parent.GetComponent<IbuttonObstacle>() != null)
            {
                var gateObs = parent.GetComponent<IbuttonObstacle>();
                gateObs.OnGreenButtonClick();
            }
        }

        else if(other.GetComponent<Spike>() != null)
        {
            var parent = other.GetComponent<Spike>().ReturnObstacleParent();

            if(parent.GetComponent<IbuttonObstacle>() != null)
            {
                var spikeObs = parent.GetComponent<IbuttonObstacle>();
                spikeObs.Collide(Player);
            }
        }
    }
}

