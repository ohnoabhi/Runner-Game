using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopHudItem : MonoBehaviour
{
    [SerializeField] private Image Icon;

    [SerializeField] private TMP_Text text;

    [SerializeField] CollectableType collectabletype;

    private void Start()
    {
        var collectable = CollectablesManager.Get(collectabletype);

        if (Icon)
        {
            Icon.sprite = CollectablesManager.GetIcon(collectabletype);
        }

        UpdateText(collectable);

        CollectablesManager.RegisterForUpdate(OnUpdate);
    }

    private void OnDestroy()
    {
        CollectablesManager.DeRegisterForUpdate(OnUpdate);
    }

    private void OnUpdate(int amount)
    {
        UpdateText(amount);
    }

    public void UpdateText(int amount)
    {
        if (text)
        {
            text.text = amount.ToString();
        }
    }
}