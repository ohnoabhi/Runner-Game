using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

public class WallRunFinisher : GameFinisher
{
    [SerializeField] private Transform wallParent;
    [SerializeField] private Wall wallPrefab;

    [SerializeField] private int unlockIntervel = 2;
    private int unlockedWalls;

    [Button]
    private void CreateWalls(int count)
    {
        var offset = 4;
        foreach (Transform child in wallParent)
        {
            if (Application.isPlaying)
            {
                Destroy(child.gameObject);
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }
        }

        for (var i = 0; i < count; i++)
        {
            Instantiate(wallPrefab, new Vector3(0, 0, wallParent.position.z + (i * offset)), Quaternion.identity,
                wallParent);
        }
    }

    private void Start()
    {
        unlockedWalls = StatsManager.Get(StatType.PlayerStat) <= 0
            ? 0
            : StatsManager.Get(StatType.PlayerStat) / unlockIntervel;

        unlockedWalls = Mathf.Max(unlockedWalls, 2);

        for (var i = 0; i < wallParent.childCount; i++)
        {
            wallParent.GetChild(i).GetComponent<Wall>().SetLock(i >= unlockedWalls);
        }
    }

    protected override async void Finish()
    {
        base.Finish();

        var wallCount = wallParent.childCount;

        wallCount = Mathf.Min(wallCount, unlockedWalls);

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