using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    [SerializeField] private Image border;
    [SerializeField] private Image lockIcon;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color highlightColor;

    private Action<string> onClickSelect;

    public ShopItem ShopItem;

    public void Init(Action<string> onClickSelect)
    {
        var unlocked = ShopItem.IsUnlocked;
        lockIcon.gameObject.SetActive(!unlocked);
        itemIcon.gameObject.SetActive(unlocked);
        itemIcon.sprite = ShopItem.Icon;

        if (ShopItem.Key == Shop.SelectedCharacterKey)
            Select();
        else
            Deselect();

        this.onClickSelect += onClickSelect;
    }

    public void Highlight()
    {
        border.gameObject.SetActive(true);
        border.color = highlightColor;
    }

    public void Deselect()
    {
        border.gameObject.SetActive(false);
    }

    public void Select()
    {
        border.gameObject.SetActive(true);
        border.color = selectedColor;
    }

    public void OnClickSelect()
    {
        AudioManager.OnButtonClick();
        if (!ShopItem.IsUnlocked) return;
        onClickSelect?.Invoke(ShopItem.Key);
    }

    public void Unlock()
    {
        ShopItem.Unlock();
        lockIcon.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(true);
    }

    public void Deregister(Action<string> onSelectItem)
    {
        onClickSelect -= onSelectItem;
    }
}

[Serializable]
public class ShopItem
{
    public string Key => Character.name;
    public Sprite Icon;
    public Character Character;

    public bool IsUnlocked
    {
        get => PlayerPrefs.GetInt("Character_" + Key, 0) == 1;
        set => PlayerPrefs.SetInt("Character_" + Key, value ? 1 : 0);
    }


    public void Unlock()
    {
        IsUnlocked = true;
    }
}