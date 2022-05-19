using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : BaseScreen
{
    [SerializeField] Button shopBtn, closeShopBtn;

    private void Awake()
    {
        shopBtn.onClick.RemoveAllListeners();
        shopBtn.onClick.AddListener(() => OnClickShop());

        closeShopBtn.onClick.RemoveAllListeners();
        closeShopBtn.onClick.AddListener(() => OnClickCloseShop());
    }

    private void OnClickCloseShop()
    {
        GameManager.Instance.CurrentState = GameManager.GameStates.MainMenu;
        ScreenController.instance.Hide("Shop");
    }

    private void OnClickShop()
    {
        GameManager.Instance.CurrentState = GameManager.GameStates.MainMenu;
        ScreenController.instance.Show("Shop");
    }
}