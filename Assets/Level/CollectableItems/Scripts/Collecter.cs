using UnityEngine;

public class Collecter : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var collectable = other.GetComponent<CollectableItem>();

        if (collectable == null) return;
        collectable.OnCollide(playerController);
    }
}