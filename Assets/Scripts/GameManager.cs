using System;
using UnityEngine;

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
        var levelData = LevelDatabase.Get().Levels[Level - 1];
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

    public void StartGame()
    {
        ScreenController.instance.Show("Game");
        LoadLevel();
        if (player) Destroy(player.gameObject);
        player = Instantiate(playerPrefab);
        player.name = "Player";
        player.State = Player.PlayerState.Running;
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
            else ScreenController.instance.Show("Win");
        }
        else ScreenController.instance.Show("Lose");
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

    public void ShowFinisherUI()
    {
        ScreenController.instance.Show("Finisher");
    }

    public void GameOver(bool win)
    {
        ClearLevel();
        ScreenController.instance.Show(win ? "Win" : "Lose");
    }
}