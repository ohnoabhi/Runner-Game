using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Stats;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class WallRunFinisher : GameFinisher
{
    [SerializeField] private int playerSpeed = 25;
    [SerializeField] private Transform wallParent;
    [SerializeField] private Wall wallPrefab;
    [SerializeField] private float ofssetZ = 8;
    [SerializeField] private int damage = 10;

    [SerializeField] private int unlockIntervel = 2;
    private int unlockedWalls;

#if UNITY_EDITOR
    [Button]
    private async void CreateWalls(int count)
    {
        for (int i = 0; i < wallParent.childCount; i++)
        {
            if (Application.isPlaying)
            {
                Destroy(wallParent.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(wallParent.GetChild(i).gameObject);
            }

            await Task.Yield();
        }

        for (var i = 0; i < count; i++)
        {
            var instance = (Wall) PrefabUtility.InstantiatePrefab(wallPrefab);
            instance.transform.localPosition = new Vector3(0, 0, wallParent.position.z + (i * ofssetZ));
            instance.transform.parent = wallParent;
            // Instantiate(wallPrefab, new Vector3(0, 0, wallParent.position.z + (i * ofssetZ)), Quaternion.identity,
            //     wallParent);
        }
    }
#endif

    private void Start()
    {
        unlockedWalls = StatsManager.Get(StatType.PlayerStat) <= 0
            ? 0
            : StatsManager.Get(StatType.PlayerStat) / unlockIntervel;

        unlockedWalls = Mathf.Max(unlockedWalls, 2);

        for (var i = 0; i < wallParent.childCount; i++)
        {
            var wall = wallParent.GetChild(i).GetComponent<Wall>();
            wall.Init(i >= unlockedWalls, i + 1, i * unlockIntervel, damage);
        }
    }

    protected override async void Finish()
    {
        base.Finish();

        var wallCount = wallParent.childCount;

        wallCount = Mathf.Min(wallCount, unlockedWalls);
        var wallReached = 0;
        for (var i = 0; i < wallCount; i++)
        {
            var targetPosition = wallParent.GetChild(i).transform.position + new Vector3(0, 0, ofssetZ * 0.5f);
            while (PlayerController.transform.position != targetPosition)
            {
                PlayerController.transform.position =
                    Vector3.MoveTowards(PlayerController.transform.position, targetPosition,
                        playerSpeed * Time.deltaTime);
                await Task.Yield();
            }

            wallReached++;
            if (PlayerController.Health < damage) break;
        }

        PlayerController.Animator.SetTrigger("Idle");
        await Task.Delay(1500);

        GameManager.Instance.GameOver(true, 0, wallReached);
    }
}