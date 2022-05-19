using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        MainMenu,
        Playing,
        Paused,
        Win,
        Lose
    }

    public static GameManager Instance;
    public static Action<int, int, int> OnPlayerHealthChange;
    public static Action<int> OnPlayerHealthLevelChange;

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
    }

    private void ClearLevel()
    {
        foreach (Transform child in levelParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoadLevel()
    {
        var levelData = LevelDatabase.Get().Levels[Level - 1];
        var objectDatabase = LevelObjectDatabase.Get();
        foreach (var levelItemData in levelData.LevelItems)
        {
            var itemPrefab = objectDatabase.Get(levelItemData.ReferenceID);

            var instance = Instantiate(itemPrefab, levelItemData.Position, Quaternion.identity, levelParent);
            instance.SetData(levelItemData);
        }
    }

    public void StartGame()
    {
        ScreenController.instance.Show("");
    }

    public void OnFinish()
    {
    }
}