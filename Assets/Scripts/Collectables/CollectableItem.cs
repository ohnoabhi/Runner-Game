using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public CollectableItemType type;
    public int amount = 1;

    public enum CollectableItemType
    {
        Health
    }
}