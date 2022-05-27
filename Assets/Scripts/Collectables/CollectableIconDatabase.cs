using System;
using UnityEngine;

namespace Collectables
{
    [CreateAssetMenu(fileName = "CollectableIcons", menuName = "Collectables/Icon", order = 0)]
    public class CollectableIconDatabase : ScriptableObject
    {
        public Sprite Default;
        public CollectableIcon[] Icons;

        public static CollectableIconDatabase Get()
        {
            return Resources.Load<CollectableIconDatabase>("CollectableIcons");
        }
    }

    [Serializable]
    public class CollectableIcon
    {
        public CollectableType Type;
        public Sprite Icon;
    }
}