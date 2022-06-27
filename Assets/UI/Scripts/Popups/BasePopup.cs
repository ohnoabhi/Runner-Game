using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public void Hide()
    {
        if (gameObject)
        {
            gameObject.SetActive(false);
            OnHide();
        }
    }

    protected virtual void OnShow()
    {
    }

    protected virtual void OnHide()
    {
    }
}