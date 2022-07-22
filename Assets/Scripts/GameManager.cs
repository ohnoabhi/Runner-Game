using System;
using System.Threading.Tasks;
using Challenges;
using Sirenix.OdinInspector;
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
    public static Action<int> OnPlayerHealthChange;
    [BoxGroup("Level")] [SerializeField] private bool CustomLevel;

    [BoxGroup("Level")] [ShowIf("CustomLevel")] [SerializeField]
    private int LevelNumber;
    [BoxGroup("Level")]
    [SerializeField] private bool randomEndType;

    [SerializeField] private PlayerController PlayerControllerPrefab;
    [SerializeField] private CameraFollower cameraFollower;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 6, -10);
    [SerializeField] private Vector3 cameraRotation = new Vector3(10, 0, 0);
    [SerializeField] private Price winBasePrice;
    [SerializeField] private float winIncrementPercentage;
    [SerializeField] private GameScreen gameScreen;

    [BoxGroup("Health")] [SerializeField] public int HealthRequiredForIncrement = 20;
    [BoxGroup("Health")] [SerializeField] public float HealthIncrementPercentage = 0.2f;
    [BoxGroup("Character Size")] public float PlayerStartSize;

    [BoxGroup("Character Size")] public float PlayerMaxSize;

    public static Action ResetObstacleEvent;

    private GameFinisher finisher;

    public PlayerController PlayerController { get; private set; }

    public static int Level
    {
        // get => 1;
        get => Instance && Instance.CustomLevel && Instance.LevelNumber > 0
            ? Instance.LevelNumber
            : PlayerPrefs.GetInt("Level", 1);
        set => PlayerPrefs.SetInt("Level", value);
    }

    public GameStates CurrentState = GameStates.MainMenu;

    private Transform levelParent;

    private void Awake()
    {
        Instance = this;
        levelParent = new GameObject("Level").transform;
    }

    private async void Start()
    {
        CurrentState = GameStates.MainMenu;
        if (!string.IsNullOrEmpty(Username.Name))
            PopupController.instance.Show("Loading");
        await PoolManager.LoadItems();
        PopupController.instance.Hide("Loading");
    }

    public void RestartGame()
    {
        ClearLevel();
    }

    private async Task ClearLevel()
    {
        ResetObstacleEvent?.Invoke();
        if (PlayerController) Destroy(PlayerController.gameObject);
        await Task.Yield();
        if (finisher) Destroy(finisher.gameObject);
        finisher = null;
        await Task.Yield();
        PoolManager.Instance.ClearLevelObjects();
    }

    private async Task LoadLevel()
    {
        await ClearLevel();
        var levelData = LevelDatabase.Get().Levels[GetLevelIndex()];
        var objectDatabase = LevelObjectDatabase.Get();
        var lastEnd = Vector3.zero;
        foreach (var levelItemData in levelData.LevelItems)
        {
            // var instance = PoolManager.Instance.GetLevelObject(objectDatabase.Get(levelItemData.ReferenceID));
            var instance = PoolManager.Instance.GetLevelObject(levelItemData.Item);

            var tempData = levelItemData;
            tempData.Position += new Vector3(0, 0, -6);
            if (instance.Layout == LevelObject.HorizontalLayout.Single)
                tempData.Position.x = 0;
            instance.transform.position = tempData.Position;
            instance.transform.rotation = Quaternion.identity;
            instance.transform.SetParent(levelParent);
            instance.SetData(tempData);
            if (instance.IsPlatform)
            {
                lastEnd = instance.End.position;
            }
            // await Task.Yield();
        }

        var values = Enum.GetValues(typeof(LevelEndType));
        var gameFinisherPrefab = objectDatabase.GetFinisher(randomEndType
            ? (LevelEndType) values.GetValue(Random.Range(0, values.Length))
            : levelData.EndType);
        if (gameFinisherPrefab)
        {
            finisher = Instantiate(gameFinisherPrefab, lastEnd, Quaternion.identity, levelParent);
        }
    }

    private static int GetLevelIndex()
    {
        var level = Level - 1;
        var levels = LevelDatabase.Get().Levels;
        if (levels.Count <= 0) return -1;
        if (level == 0 || levels.Count == 1) return 0;
        return levels.Count > level ? level : Random.Range(Mathf.FloorToInt(levels.Count * 0.8f), levels.Count);
    }

    public async void StartGame()
    {
        TinySauce.OnGameStarted(Level.ToString());
        PopupController.instance.Show("Loading");
        ScreenController.instance.Show("Game");
        await LoadLevel();
        if (PlayerController) Destroy(PlayerController.gameObject);
        PlayerController = Instantiate(PlayerControllerPrefab);
        PlayerController.name = "Player";
        PlayerController.Init();
        PlayerController.State = PlayerController.PlayerState.Running;
        // var playerCharacterManager = player.GetComponent<PlayerCharacterManager>();
        PlayerController.UI.SetCamera(cameraFollower.transform);
        OnPlayerCashCollected(0);
        cameraFollower.transform.position = PlayerController.transform.position + cameraOffset;
        cameraFollower.transform.rotation = Quaternion.Euler(cameraRotation);
        cameraFollower.Target = PlayerController.transform;
        cameraFollower.SetOffset(cameraOffset, Quaternion.Euler(cameraRotation), false);
        CurrentState = GameStates.Playing;
        PopupController.instance.Hide();
    }

    public void UpdateCameraHeight(float percentage)
    {
        if (PlayerController.State != PlayerController.PlayerState.Running) return;
        var offset = cameraOffset;
        offset.y += offset.y * percentage * 0.2f;
        cameraFollower.SetOffset(offset);
    }

    public void OnFinish(bool win)
    {
        if (CurrentState != GameStates.Playing) return;
        if (win)
        {
            PlayerController.State = PlayerController.PlayerState.Finish;
            ScreenController.instance.Show("Finisher");
            if (finisher)
            {
                CurrentState = GameStates.Finisher;
                finisher.StartFinisher(PlayerController);
            }
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

    public void GameOver(bool win, float delay = 0, int multiplier = 1, bool isChest = false)
    {
        TinySauce.OnGameFinished(win, 0);
        if (win)
        {
            Level++;
            ChallengeManager.Instance.UpdateChallenge(ChallengeType.WinLevels);
        }

        CurrentState = win ? GameStates.Win : GameStates.Lose;
        ScreenController.instance.SetFinisherUI();
        ScreenController.instance.Show(win ? "Win" : "Lose", delay, GetWinAmount(), multiplier, isChest);
        ClearLevel();
    }

    public Price GetWinAmount()
    {
        return new Price()
        {
            Type = winBasePrice.Type,
            Amount = Mathf.RoundToInt(PlayerController.CashCollected + winBasePrice.Amount + (winBasePrice.Amount *
                ((StatsManager.Get(StatType.RewardMultiplier) - 1) *
                 winIncrementPercentage)))
        };
    }

    public void OnPlayerCashCollected(int amount)
    {
        gameScreen.UpdateCash(amount);
    }
}