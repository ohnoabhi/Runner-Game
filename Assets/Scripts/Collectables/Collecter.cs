using UnityEngine;

public class Collecter : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var collectable = other.GetComponent<CollectableItem>();

        if (collectable == null) return;

        switch (collectable.type)
        {
            case CollectableItem.CollectableItemType.Health:
                playerHealth.GainHealth(collectable.amount);
                break;
            case CollectableItem.CollectableItemType.Cash:
                CollectablesManager.Add(CollectableType.Cash, 1);
                break;
        }

        Destroy(collectable.gameObject);
    }
}