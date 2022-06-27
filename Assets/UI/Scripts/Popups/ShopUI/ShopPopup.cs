using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopPopup : BasePopup
{
    [SerializeField] private Price price;
    [SerializeField] private float priceincrementPercentage = 0.25f;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Image collectableIcon;
    [SerializeField] private TextMeshProUGUI priceAmount;

    [SerializeField] private ShopItemView[] items;


    private Price currentPrice
    {
        get
        {
            var price = new Price()
            {
                Amount = this.price.Amount,
                Type = this.price.Type
            };
            price.Amount += Mathf.RoundToInt(price.Amount * (Shop.PurchaseCount * priceincrementPercentage));
            return price;
        }
    }


    private void Start()
    {
        collectableIcon.sprite = CollectablesManager.GetIcon(currentPrice.Type);
        ValidateUnlockButton();
    }

    protected override void OnShow()
    {
        var i = 0;
        foreach (var item in items)
        {
            item.Init(OnSelectItem);
        }
    }

    private void OnSelectItem(string key)
    {
        Shop.SelectedCharacterKey = key;
        foreach (var item in items)
        {
            if (item.ShopItem.Key == key)
                item.Select();
            else
                item.Deselect();
        }
    }

    protected override void OnHide()
    {
        foreach (var item in items)
        {
            item.Deregister(OnSelectItem);
        }
    }


    public async void Unlock()
    {
        var lockedItems = GetLocked();
        if (!currentPrice.IsAffordable() || lockedItems.Length == 0)
        {
            ValidateUnlockButton();
            return;
        }

        unlockButton.interactable = false;
        var unlockedId = 0;
        if (lockedItems.Length > 1)
        {
            var delay = 100;
            for (var i = 0; i < 20; i++)
            {
                var randomId = lockedItems[Random.Range(0, lockedItems.Length)];
                items[randomId].Highlight();
                await Task.Delay(delay);
                items[randomId].Deselect();
                delay += 5;
            }

            unlockedId = lockedItems[Random.Range(0, lockedItems.Length)];
        }

        foreach (var item in items)
        {
            item.Deselect();
        }

        currentPrice.Pay();
        items[unlockedId].Unlock();
        OnSelectItem(items[unlockedId].ShopItem.Key);
        Shop.PurchaseCount++;
        ValidateUnlockButton();
    }

    private void ValidateUnlockButton()
    {
        priceAmount.text = currentPrice.Amount.ToString();
        unlockButton.interactable = currentPrice.IsAffordable() && GetLocked().Length != 0;
    }

    private int[] GetLocked()
    {
        var ids = new List<int>();
        var id = 0;
        foreach (var item in items)
        {
            if (!item.ShopItem.IsUnlocked)
                ids.Add(id);
            id++;
        }

        return ids.ToArray();
    }
}