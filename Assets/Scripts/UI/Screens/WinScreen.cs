using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{
    [SerializeField] Button winContinueBtn;

    private void Awake()
    {
        winContinueBtn.onClick.RemoveAllListeners();
        winContinueBtn.onClick.AddListener(() => { ScreenController.instance.Show("Menu"); });
    }
}