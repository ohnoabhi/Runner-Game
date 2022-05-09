using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public void Show()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void Hide()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
