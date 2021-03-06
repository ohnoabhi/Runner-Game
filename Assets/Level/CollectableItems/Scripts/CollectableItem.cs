using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public CollectableItemType type;
    public int amount = 1;
    [SerializeField] private bool playAudio = true;

    public enum CollectableItemType
    {
        Health,
        Cash,
        Coin
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnCollide(PlayerController playerController)
    {
        switch (type)
        {
            case CollectableItemType.Health:
                if (playAudio)
                    AudioManager.Play("MeatPickup");
                playerController.GainHealth(amount);
                break;
            case CollectableItemType.Cash:
                if (playAudio)
                    AudioManager.Play("CashPickup");
                // CollectablesManager.Add(CollectableType.Cash, 1);
                playerController.CollectCoin(amount);
                break;
            case CollectableItemType.Coin:
                if (playAudio)
                    AudioManager.Play("CoinClaim");
                // CollectablesManager.Add(CollectableType.Cash, 1);
                playerController.CollectCoin(amount);
                break;
        }
        Hide();
    }
}