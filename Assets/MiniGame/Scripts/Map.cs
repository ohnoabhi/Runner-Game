using UnityEngine;


namespace maps
{
    public class Map : MonoBehaviour
    {
        public int MapId;
        public CreatureItem[] Creatures;
        public bool completed
        {
            get
            {
                foreach (var creature in Creatures)
                {
                    if (!creature.IsUnlocked)
                        return false;
                }
                return true;
            }
        }
    }
}
