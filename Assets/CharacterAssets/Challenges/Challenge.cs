using UnityEngine;

namespace Challenges
{
    public enum ChallengeType
    {
        KillBoss,
        PlayLevel,
        KillEnemy
    }

    public static class ChallengeExtension
    {
        public static string GetDescription(this ChallengeType challengeType, int amount)
        {
            switch (challengeType)
            {
                case ChallengeType.KillBoss:
                    return "Kill " + amount + " Bosses";
                case ChallengeType.PlayLevel:
                    return "Play " + amount + " Levels";
                case ChallengeType.KillEnemy:
                    return "Kill " + amount + " Enemies";
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
            Reward = challenge.Reward;
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
            return Type.ToString() + "[" + Required + "]";
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