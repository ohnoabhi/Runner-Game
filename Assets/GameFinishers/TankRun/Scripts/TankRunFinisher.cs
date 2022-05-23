using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TankRunFinisher : GameFinisher
{
    [SerializeField] private Transform tankParent;

    protected override async void Finish()
    {
        base.Finish();

        var tankCount = tankParent.childCount;

        var playerHealth = player.GetComponent<PlayerHealth>();
        var tankToReachPercentage = playerHealth.CurrentHealth / playerHealth.MaxHealth;

        var tankIndex = Mathf.FloorToInt(tankCount * tankToReachPercentage);

        var tank = tankParent.GetChild(tankIndex);

        var targetPosition = tank.transform.position + new Vector3(0, 0, 2);
        while (player.transform.position != targetPosition)
        {
            player.transform.position =
                Vector3.MoveTowards(player.transform.position, targetPosition, 5 * Time.deltaTime);
            await Task.Yield();
        }

        await Task.Delay(300);

        GameManager.Instance.GameOver(true);
    }
}