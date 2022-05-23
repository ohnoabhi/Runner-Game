using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WallRunFinisher : GameFinisher
{
    [SerializeField] private Transform wallParent;

    protected override async void Finish()
    {
        base.Finish();

        var wallCount = wallParent.childCount;

        var playerHealth = player.GetComponent<PlayerHealth>();
        var wallToReachPercentage = playerHealth.CurrentHealth / playerHealth.MaxHealth;

        var wallIndex = Mathf.FloorToInt(wallCount * wallToReachPercentage);

        var wall = wallParent.GetChild(wallIndex);

        var targetPosition = wall.transform.position + new Vector3(0, 0, 2);
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