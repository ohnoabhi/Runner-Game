using UnityEngine;

namespace Challenges
{
    public enum ChallengeType
    {
        WinLevels,
        KillBoss,
        DestroyWalls,
        DestroyGlass
    }

    public static class ChallengeExtension
    {
        public static string GetDescription(this ChallengeType challengeType, int amount)
        {
            switch (challengeType)
            {
                case ChallengeType.WinLevels:
                    return "Win " + amount + " Levels";
                case ChallengeType.KillBoss:
                    return "Kill " + amount + " Bosses";
                case ChallengeType.DestroyWalls:
                    return "Destroy " + amount + " Walls";
                case ChallengeType.DestroyGlass:
                    return "Destroy " + amount + " Glasses";
            }

            return "";
        }

        public static Sprite GetIcon(this ChallengeType challengeType)
        {
            return ChallengeManager.GetIcon(challengeType);
        }
    }

    [System.Serializable]
    public class Challenge
    {
        // public Color backgroundColor;
        public ChallengeType Type;
        public Reward Reward;
        public int Required;
        [HideInInspector] public int Completed;

        public bool IsCompleted => Completed >= Required;
        [HideInInspector] public bool IsClaimed;

        public Challenge(Challenge challenge)
        {
            Type = challenge.Type;
            Reward = new Reward(challenge.Reward);
            Required = challenge.Required;
            Completed = 0;
            IsClaimed = false;
        }

        public void Update(int amount)
        {
            if (IsCompleted) return;
            Completed += amount;
            if (Completed > Required) Completed = Required;
        }

        public void Claim()
        {
            if (!IsCompleted) return;
            if (IsClaimed) return;
            Reward.Claim();
            IsClaimed = true;
            ChallengeManager.GetSpecialReward().Update();
            ChallengeManager.Instance.Save();
        }

        public override string ToString()
        {
            return Type + "[" + Required + "]";
        }
    }

    [System.Serializable]
    public class SpecialReward
    {
        public Reward Reward;
        [HideInInspector] public int Completed;
        public bool IsCompleted => Completed >= required;
        [HideInInspector] public bool IsClaimed;

        private int required = 8;


        public void Update(int amount = 1)
        {
            if (IsCompleted) return;
            Completed += amount;
            if (Completed > required) Completed = required;
            ChallengeManager.Instance.Save();
        }

        public void Claim()
        {
            if (!IsCompleted) return;
            if (IsClaimed) return;
            Reward.Claim();
            IsClaimed = true;
            ChallengeManager.Instance.AssignNewSpecialReward();
        }
    }
}