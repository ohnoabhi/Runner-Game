using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Stats;
using TMPro;
using UnityEngine;

public class TankFinisher : GameFinisher
{
    [SerializeField] private int playerSpeed = 10;
    [SerializeField] private Transform tankParent;
    [SerializeField] private TankFinisherItem tankPrefab;
    [SerializeField] private GameObject tankLockedPrefab;
    [SerializeField] private float offsetZ = 16;
    [SerializeField] private int unlockIntervel = 2;
    [SerializeField] private int damage = 20;

    private TankFinisherItem[] tanks;

    private void Start()
    {
        var stat = StatsManager.Get(StatType.PlayerStat);
        var count = stat - 3 <= 0 ? 2 : 1 + (stat / unlockIntervel);
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

        tanks = new TankFinisherItem[count];
        for (var i = 0; i < count; i++)
        {
            var instance = Instantiate(tankPrefab, new Vector3(0, 0, tankParent.position.z + (i * offsetZ)),
                Quaternion.Euler(0, 180, 0),
                tankParent);
            var textMeshPro = instance.GetComponentInChildren<TextMeshPro>();
            textMeshPro.text = "X" + (i + 1);
            textMeshPro.color = Color.white;
            tanks[i] = instance;
            await Task.Yield();
        }

        var locked = Instantiate(tankLockedPrefab, new Vector3(0, 0, tankParent.position.z + (count * offsetZ)),
            Quaternion.Euler(0, 180, 0),
            tankParent);
        locked.GetComponentInChildren<TextMeshPro>().text = "LVL " + (count * unlockIntervel);
    }

    protected override async void Finish()
    {
        base.Finish();

        var tankReached = 0;
        foreach (var tank in tanks)
        {
            var targetPosition = tank.transform.position + new Vector3(0, 0, offsetZ * 0.5f);
            var damageTook = false;
            while (PlayerController.transform.position != targetPosition)
            {
                PlayerController.transform.position =
                    Vector3.MoveTowards(PlayerController.transform.position, targetPosition,
                        playerSpeed * Time.deltaTime);
                if (!damageTook && PlayerController.transform.position.z >= tank.transform.position.z - 1f)
                {
                    damageTook = true;
                    var textMeshPro = tank.GetComponentInChildren<TextMeshPro>();
                    textMeshPro.color = Random.ColorHSV(0.1f, 0.8f, 1f, 1f, 0.5f, 1f);
                    tank.PlayParticles();
                    PlayerController.TakeDamage(damage);
                }

                await Task.Yield();
            }

            tankReached++;
            if (PlayerController.Health <= damage) break;
        }

        PlayerController.Animator.SetTrigger("Idle");
        await Task.Delay(1500);

        GameManager.Instance.GameOver(true, 0, tankReached);
    }
}