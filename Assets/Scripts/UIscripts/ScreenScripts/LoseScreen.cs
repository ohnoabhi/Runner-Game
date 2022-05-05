using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : BaseScreen
{
    [SerializeField]
    Button loseContinueBtn;

    private void Awake()
    {
        loseContinueBtn.onClick.RemoveAllListeners();
        loseContinueBtn.onClick.AddListener(() => OnClickLoseContinue());
    }

    private void OnClickLoseContinue()
    {
        Hide();
        GameManager.instance.Restartgame();
    }
}
