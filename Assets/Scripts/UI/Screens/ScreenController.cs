using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public static ScreenController instance;

    [SerializeField] private ScreenItem[] screenItems;

    [SerializeField] private string defaultScreen = "";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Show(defaultScreen);
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

[Serializable]
class ScreenItem
{
    public string name;
    public BaseScreen screen;
    public bool topHud;
}