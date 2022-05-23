using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : BaseScreen
{
    [SerializeField] Button loseContinueBtn;

    private void Awake()
    {
        loseContinueBtn.onClick.RemoveAllListeners();
        loseContinueBtn.onClick.AddListener(() =>
        {
            ScreenController.instance.Show("Menu");
        });
    }
}