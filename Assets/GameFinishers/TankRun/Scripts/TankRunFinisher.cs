using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Stats;
using TMPro;
using UnityEngine;

public class TankRunFinisher : GameFinisher
{
    [SerializeField] private int playerSpeed = 10;
    [SerializeField] private Transform tankParent;
    [SerializeField] private GameObject tankPrefab;
    [SerializeField] private float offsetZ = 16;
    [SerializeField] private int unlockIntervel = 2;

    private void Start()
    {
        var count = StatsManager.Get(StatType.PlayerStat) <= 0
            ? 0
            : StatsManager.Get(StatType.PlayerStat) / unlockIntervel;
        count = Mathf.Max(count, 2);
        CreateTanks(count);
    }

    [Button]
    private async void CreateTanks(int count)
    {
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

            await Task.Yield();
        }

        for (var i = 0; i < count; i++)
        {
            var instance = Instantiate(tankPrefab, new Vector3(0, 0, tankParent.position.z + (i * offsetZ)),
                Quaternion.Euler(0, 180, 0),
                tankParent);
            instance.GetComponentInChildren<TextMeshPro>().text = "X" + (i + 1);
            await Task.Yield();
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

        var targetPosition = tank.transform.position + new Vector3(0, 0, offsetZ * 0.5f);
        while (player.transform.position != targetPosition)
        {
            player.transform.position =
                Vector3.MoveTowards(player.transform.position, targetPosition, playerSpeed * Time.deltaTime);
            await Task.Yield();
        }


        var character = player.GetComponent<PlayerCharacterManager>().Character;
        character.Animator.SetTrigger("Roar");
        await Task.Delay(1500);

        GameManager.Instance.GameOver(true);
    }
}