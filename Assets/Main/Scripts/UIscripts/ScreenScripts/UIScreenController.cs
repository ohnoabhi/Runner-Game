using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenController : MonoBehaviour
{
    public static UIScreenController instance;

    [SerializeField]
    private ScreenItem[] screenItems;

    [SerializeField]
    private string defaultScreen = "";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    public void Show(string name)
    {
        Show(name, 0, Array.Empty<object>());        
    }

    public void Show(string name, float delay = 0, params object[] args)
    {
        foreach (var screenItem in screenItems)
        {
            if (screenItem.name == name)
            {
                screenItem.screen.Show();
                TopHudController.Enable(screenItem.topHud);
            }
            else 
                screenItem.screen.Hide();
        }
    }

    public void Hide(string name)
    {
        foreach (var screenItem in screenItems)
        {
            if (screenItem.name == name)
                screenItem.screen.Hide();
        }

        foreach (var screenItem in screenItems)
        {
            if (screenItem.name == "MainMenu")
            {
                screenItem.screen.Show();
                TopHudController.Enable(screenItem.topHud);
            }
        }


    }


}

[System.Serializable]
class ScreenItem
{
    public string name;
    public BaseScreen screen;
    public bool topHud;
}
