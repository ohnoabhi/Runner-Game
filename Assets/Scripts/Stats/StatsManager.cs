using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    public class StatsManager : MonoBehaviour
    {
        private static StatsManager instance;
        [SerializeField] private Stat[] stats;

        private void Awake()
        {
            instance = this;
        }

        public static int Get(StatType type)
        {
            return (from stat in instance.stats where stat.Type == type select stat.Progress).FirstOrDefault();
        }

        public static void Update(StatType type, int amount = 1)
        {
            foreach (var stat in instance.stats)
            {
                if (stat.Type != type) continue;
                stat.Progress += amount;
                break;
            }
        }

        public static Price GetPrice(StatType type)
        {
            return (from stat in instance.stats where stat.Type == type select stat.Price).FirstOrDefault();
        }
    }

    public enum StatType
    {
        PlayerStat,
        RewardMultiplier
    }

    [Serializable]
    public class Stat
    {
        public StatType Type;
        [PropertyRange(0, 1)] public float PriceIncrementPercentage;
        public int BasePrice;
        public CollectableType PriceType;

        [HideInInspector]
        public Price Price =>
            new Price()
            {
                Type = PriceType,
                Amount = Mathf.RoundToInt(BasePrice +
                                          BasePrice * (PriceIncrementPercentage *
                                                       ((GameManager.Level - 1) + (Progress - 1))))
            };

        public int Progress
        {
            get => PlayerPrefs.GetInt("STAT_" + Type, 1);
            set => PlayerPrefs.SetInt("STAT_" + Type, value);
        }
    }
}