using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseScreen
{
    [SerializeField] Button playBtn;

    private void Awake()
    {
        playBtn.onClick.RemoveAllListeners();
        playBtn.onClick.AddListener(() => { GameManager.Instance.StartGame(); });
    }
}