using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Canvas menuPanel;

    [SerializeField]
    Canvas winPanel;

    [SerializeField]
    Canvas losePanel;

    [SerializeField]
    Canvas pausepanel;

    [SerializeField]
    Button playBtn, pauseBtn, stopPauseBtn, winContinueBtn, loseContinueBtn, restartBtn, exitBtn;

    public static Action<bool> OnGameEnd;

    

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        OnGameEnd += GameEnd;
    }

    private void OnDisable()
    {
        OnGameEnd -= GameEnd;
    }

    void Start()
    {
        playBtn.onClick.RemoveAllListeners();
        playBtn.onClick.AddListener(() => OnClickPlay());

        pauseBtn.onClick.RemoveAllListeners();
        pauseBtn.onClick.AddListener(() => OnClickPause());

        stopPauseBtn.onClick.RemoveAllListeners();
        stopPauseBtn.onClick.AddListener(() => OnClickStopPause());

        winContinueBtn.onClick.RemoveAllListeners();
        winContinueBtn.onClick.AddListener(() => OnClickWinContinue());

        loseContinueBtn.onClick.RemoveAllListeners();
        loseContinueBtn.onClick.AddListener(() => OnClickLoseContinue());

        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(() => MainMenu());

        restartBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.AddListener(() => RestartGame());
    }

    public void MainMenu() //works same as restart function need to be fixed
    {
        RestartGame();
        menuPanel.enabled = true;
        GameManager.instance.currentState = GameManager.GameStates.MainMenu;
    }

    private void OnClickLoseContinue()
    {
        losePanel.enabled = false;

        GameManager.instance.currentState = GameManager.GameStates.MainMenu;

        RestartGame();
    }

    private void OnClickWinContinue()
    {
        winPanel.enabled = false;

        GameManager.instance.currentState = GameManager.GameStates.MainMenu;

        MainMenu();
    }

    private void OnClickStopPause()
    {
        pausepanel.enabled = false;

        GameManager.instance.currentState = GameManager.GameStates.Playing;
    }

    private void OnClickPause()
    {
        pausepanel.enabled = true;

        GameManager.instance.currentState = GameManager.GameStates.Paused;
        
    }

    private void OnClickPlay()
    {
        menuPanel.enabled = false;

        GameManager.instance.currentState = GameManager.GameStates.Playing;

    }

    private void Onwin()
    {
        winPanel.enabled = true;

        GameManager.instance.currentState = GameManager.GameStates.Win;
    }

    private void OnLose()
    {
        losePanel.enabled = true;

        GameManager.instance.currentState = GameManager.GameStates.Lose;
    }

    private void GameEnd(bool win)
    {
        if (win)
            Onwin();
        else
            OnLose();
    }

    public void RestartGame()
    {
        GameManager.instance.Restartgame();
        DisableAllMenus();
        GameManager.instance.currentState = GameManager.GameStates.Playing;
    }

    public void DisableAllMenus()
    {
        menuPanel.enabled = false;
        pausepanel.enabled = false;
        winPanel.enabled = false;
        losePanel.enabled = false;
    }

}
