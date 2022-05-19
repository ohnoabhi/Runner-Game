using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : BasePopup
{
    [SerializeField]
    Button pauseBtn,pauseContinueBtn, restartBtn, exitBtn;
    private void Awake()
    {
        pauseBtn.onClick.RemoveAllListeners();
        pauseBtn.onClick.AddListener(() => OnClickPause());

        pauseContinueBtn.onClick.RemoveAllListeners();
        pauseContinueBtn.onClick.AddListener(() => OnClickStopPause());

        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(() => MainMenu());

        restartBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.AddListener(() => RestartGame());

    }

    private void OnClickPause()
    {
        GameManager.Instance.CurrentState = GameManager.GameStates.Paused;
        Show();

    }

    private void OnClickStopPause()
    {
        GameManager.Instance.CurrentState = GameManager.GameStates.Playing;
        Hide();
    }

    private void RestartGame()
    {
        GameManager.Instance.CurrentState = GameManager.GameStates.Playing;
        GameManager.Instance.RestartGame();
    }

    private void MainMenu()
    {
        GameManager.Instance.CurrentState = GameManager.GameStates.Playing;
        GameManager.Instance.RestartGame();
    }
}
