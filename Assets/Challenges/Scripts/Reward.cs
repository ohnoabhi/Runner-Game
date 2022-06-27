using Collectables;
using UnityEngine;

namespace Challenges
{
    [System.Serializable]
    public class Reward
    {
        public Price Price;
        [HideInInspector] public bool IsClaimed = false;

        public Reward(Reward reward)
        {
            Price = reward.Price;
            IsClaimed = false;
        }

        public void Claim()
        {
            if (IsClaimed) return;

            Price.Gain();
            IsClaimed = true;
        }

        public override string ToString()
        {
            return "Reward: " + Price.Type + "[" + Price.Amount + "]";
        }

        public Sprite Icon => CollectablesManager.GetIcon(Price.Type);
    }
}