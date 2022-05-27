using System;
using System.Linq;
using UnityEngine;


namespace maps
{
    public class Map : MonoBehaviour
    {
        public int MapId;
        public CreatureItem[] Creatures;

        public bool completed
        {
            get { return Creatures.All(creature => creature.IsUnlocked); }
        }

        public int GetCameraStartPosition()
        {
            var i = 0;
            foreach (var creature in Creatures)
            {
                if (!creature.IsUnlocked)
                {
                    return i;
                }

                i++;
            }

            return 0;
        }
    }
}