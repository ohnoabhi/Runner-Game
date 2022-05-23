using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public static PopupController instance;

    [SerializeField]
    private PopupItem[] popupItems;


    private void Awake()
    {
        instance = this;
    }

    public void Show(string name)
    {
        foreach(var popupItem in popupItems)
        {
            if(popupItem.name == name)
            {
                popupItem.popup.Show();
            }

            else
            {
                popupItem.popup.Hide();
            }
        }
    }

    public void Hide()
    {
        foreach(var popupItem in popupItems)
        {
            popupItem.popup.Hide();
        }
    }
}

[System.Serializable]
public class PopupItem
{
    public string name;
    public BasePopup popup;
    public bool hideTopHud = false;

}
