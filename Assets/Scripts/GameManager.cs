using System;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        MainMenu,
        Playing,
        Paused,
        Finisher,
        Win,
        Lose
    }

    public static GameManager Instance;
    public static Action<float, int, int> OnPlayerHealthChange;
    public static Action<int> OnPlayerHealthLevelChange;

    [SerializeField] private Player playerPrefab;
    [SerializeField] private CameraFollower cameraFollower;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 6, -10);
    [SerializeField] private Vector3 cameraRotation = new Vector3(10, 0, 0);
    [SerializeField] private Price winBasePrice;

    private GameFinisher finisher;

    public Player player { get; private set; }

    public static int Level
    {
        get => PlayerPrefs.GetInt("Level", 1);
        set => PlayerPrefs.SetInt("Level", value);
    }

    [HideInInspector] public GameStates CurrentState = GameStates.MainMenu;

    private Transform levelParent;

    private void Awake()
    {
        Instance = this;

        levelParent = new GameObject("Level").transform;
    }

    private void Start()
    {
        CurrentState = GameStates.MainMenu;
    }

    public void RestartGame()
    {
        ClearLevel();
    }

    private void ClearLevel()
    {
        if (player) Destroy(player.gameObject);
        finisher = null;
        foreach (Transform child in levelParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoadLevel()
    {
        ClearLevel();
        var levelData = LevelDatabase.Get().Levels[GetLevelIndex()];
        var objectDatabase = LevelObjectDatabase.Get();
        var lastEnd = Vector3.zero;
        foreach (var levelItemData in levelData.LevelItems)
        {
            var itemPrefab = objectDatabase.Get(levelItemData.ReferenceID);


            var instance = Instantiate(itemPrefab, levelItemData.Position, Quaternion.identity, levelParent);
            instance.SetData(levelItemData);
            if (instance.IsPlatform)
            {
                lastEnd = instance.End.position;
            }
        }

        var gameFinisherPrefab = objectDatabase.GetFinisher(levelData.EndType);
        if (gameFinisherPrefab)
        {
            finisher = Instantiate(gameFinisherPrefab, lastEnd, Quaternion.identity, levelParent);
        }
    }

    private int GetLevelIndex()
    {
        var level = Level - 1;
        var levels = LevelDatabase.Get().Levels;
        if (levels.Count <= 0) return -1;
        if (level == 0 || levels.Count == 1) return 0;
        return levels.Count > level ? level : Random.Range(Mathf.FloorToInt(levels.Count * 0.5f), levels.Count);
    }

    public void StartGame()
    {
        ScreenController.instance.Show("Game");
        LoadLevel();
        if (player) Destroy(player.gameObject);
        player = Instantiate(playerPrefab);
        player.name = "Player";
        player.State = Player.PlayerState.Running;
        player.UI.SetCamera(cameraFollower.transform);
        cameraFollower.Target = player.transform;
        cameraFollower.SetOffset(cameraOffset, Quaternion.Euler(cameraRotation), false);
        CurrentState = GameStates.Playing;
    }

    public void OnFinish(bool win)
    {
        if (win)
        {
            CurrentState = GameStates.Finisher;
            player.State = Player.PlayerState.Finish;
            ScreenController.instance.Show("Finisher");
            if (finisher)
                finisher.StartFinisher(player);
            else GameOver(true);
        }
        else GameOver(false);
    }

    public void SetCamera(Transform cameraPosition, bool isFollowCam)
    {
        if (isFollowCam)
        {
            cameraFollower.SetOffset(cameraPosition.localPosition, cameraPosition.rotation);
        }
        else
        {
            cameraFollower.MoveToPosition(cameraPosition.position, cameraPosition.rotation);
        }
    }

    public void SetCamera(bool isFollowCam)
    {
        if (isFollowCam)
        {
            cameraFollower.SetOffset(cameraFollower.transform.localPosition, cameraFollower.transform.rotation);
        }
        else
        {
            cameraFollower.MoveToPosition(cameraFollower.transform.position, cameraFollower.transform.rotation);
        }
    }

    public void ShowFinisherUI()
    {
        ScreenController.instance.Show("Finisher");
    }

    public void GameOver(bool win)
    {
        ClearLevel();
        ScreenController.instance.SetFinisherUI();
        if (win) Level++;
        ScreenController.instance.Show(win ? "Win" : "Lose", 0, GetWinAmount());
    }

    public Price GetWinAmount()
    {
        return new Price()
        {
            Type = winBasePrice.Type,
            Amount = Mathf.RoundToInt(winBasePrice.Amount +
                                      winBasePrice.Amount * (Level - 1) *
                                      (StatsManager.Get(StatType.RewardMultiplier) - 1))
        };
    }
}