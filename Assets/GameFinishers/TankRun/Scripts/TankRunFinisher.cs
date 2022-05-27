using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

public class TankRunFinisher : GameFinisher
{
    [SerializeField] private Transform tankParent;
    [SerializeField] private GameObject tankPrefab;
    [SerializeField] private int unlockIntervel = 2;

    private void Start()
    {
        CreateTanks(StatsManager.Get(StatType.PlayerStat) <= 0 ? 0 : StatsManager.Get(StatType.PlayerStat) / unlockIntervel);
    }

    [Button]
    private void CreateTanks(int count)
    {
        var offset = 4;
        foreach (Transform child in tankParent)
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
            Instantiate(tankPrefab, new Vector3(0, 0, tankParent.position.z + (i * offset)), Quaternion.identity,
                tankParent);
        }
    }

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