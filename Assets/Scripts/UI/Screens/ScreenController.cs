using System;
using System.Collections;
using System.Collections.Generic;
using UI.Screens;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public static ScreenController instance;

    [SerializeField] private ScreenItem[] screenItems;

    [SerializeField] private string defaultScreen = "";

    [SerializeField] private GameFinisherScreen finisherScreen;

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
                screenItem.screen.Show(Mathf.RoundToInt(delay * 1000), args);
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

    public GameObject SetFinisherUI(GameObject ui)
    {
        return finisherScreen.SetUI(ui);
    }
}

[Serializable]
class ScreenItem
{
    public string name;
    public BaseScreen screen;
    public bool topHud;
}