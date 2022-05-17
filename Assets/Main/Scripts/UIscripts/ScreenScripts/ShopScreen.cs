using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : BaseScreen
{
    [SerializeField]
    Button shopBtn, closeShopBtn;

    [SerializeField]
    Sets[] sets; 
    private void Awake()
    {
        shopBtn.onClick.RemoveAllListeners();
        shopBtn.onClick.AddListener(() => OnClickShop());

        closeShopBtn.onClick.RemoveAllListeners();
        closeShopBtn.onClick.AddListener(() => OnClickCloseShop());

        foreach(var set in sets)
        {
            set.setBtn.onClick.RemoveAllListeners();
            set.setBtn.onClick.AddListener(() => OnClickSet(set.setNo));
        }
        
    }

    private void OnClickSet(int setNo)
    {
        // Player.Instance.OnChangeSet?.Invoke(setNo);
    }

    private void OnClickCloseShop()
    {
        GameManager.instance.currentState = GameManager.GameStates.MainMenu;
        UIScreenController.instance.Hide("Shop");
    }

    private void OnClickShop()
    {
        GameManager.instance.currentState = GameManager.GameStates.MainMenu;
        UIScreenController.instance.Show("Shop");
    }
}

[System.Serializable]
class Sets
{
    public int setNo;
    public Button setBtn;
}
