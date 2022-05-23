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

        if (collectable != null)
        {
            if (collectable.type != CollectableType.Health)
                collectable.Collect();
            else
            {
                playerHealth.GainHealth(collectable.amount);
                Destroy(collectable.gameObject);
            }
        }
    }
}