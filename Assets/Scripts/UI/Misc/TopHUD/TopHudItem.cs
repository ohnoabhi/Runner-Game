using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopHudItem : MonoBehaviour
{
    [SerializeField] 
    private Image Icon;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    CollectableType collectabletype;
    
    private void Start()
    {
        if(CollectablesManager.GetCollectable(collectabletype) == null)
        {
            gameObject.SetActive(false);
            return;
        }

        var collectable = CollectablesManager.GetCollectable(collectabletype);

        if(Icon)
        {
            Icon.sprite = collectable.collectableImg;
        }

        UpdateText(collectable.Amount);

        CollectablesManager.instance.RegisterForUpdate(OnUpdate);
    }

    private void OnUpdate(Collectable collectable)
    {
        if(collectable.Type == collectabletype)
        {
            UpdateText(collectable.Amount);
        }
    }

    public void UpdateText(int amount)
    {
        if(text)
        {
            text.text = amount.ToString();
        }
    }
}
