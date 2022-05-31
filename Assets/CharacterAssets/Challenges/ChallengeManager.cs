using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Challenges
{
    public class ChallengeManager : MonoBehaviour
    {
        public static ChallengeManager Instance;
        public static Action<double> OnChallengeTimeUpdate;
        public static Action<Challenge[], SpecialReward> OnChallengeUpdate;
        private static string _savePath;
        private static string _specialRewardSavePath;

        [SerializeField] private Challenge[] AvailableChallenges;

        // [SerializeField] private float challengeTime = 300;
        [SerializeField] private int maxChallenges = 3;
        private double _remainingSeconds;

        [SerializeField] private Challenge[] _currentChallenges;
        // private const string DateFormat = "dd-MM-yyyy hh:mm:ss tt";

        private DateTime _nextChallengeTime;

        private SpecialReward specialReward;

        private DateTime NextChallengeTime
        {
            get
            {
                var dateString = PlayerPrefs.GetString("NextChallengeTime",
                    DateTime.Today.ToString());
                try
                {
                    var date = DateTime.Parse(dateString);
                    return date;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return DateTime.Today;
                }
            }
            set
            {
                PlayerPrefs.SetString("NextChallengeTime", value.ToString());
                _nextChallengeTime = value;
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _savePath = Application.persistentDataPath + "/challengeData.save";
            _specialRewardSavePath = Application.persistentDataPath + "/specialReward.save";

            specialReward = LoadSpecialReward();
            AssignNewSpecialReward();

            _nextChallengeTime = NextChallengeTime;

            if (_nextChallengeTime <= DateTime.Today)
            {
                AssignNewChallenges(true);
            }
            else
            {
                _currentChallenges = Load();
                if (_currentChallenges == null) AssignNewChallenges(true);
                else OnChallengeUpdate?.Invoke(_currentChallenges, specialReward);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                foreach (var challenge in _currentChallenges)
                {
                    UpdateChallenge(challenge.Type, challenge.Required);
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                AssignNewChallenges(true);
            }

            var today = DateTime.Now;
            if (_nextChallengeTime > today)
            {
                _remainingSeconds = _nextChallengeTime.Subtract(today).TotalSeconds;
                OnChallengeTimeUpdate?.Invoke(_remainingSeconds);
            }
            else
            {
                if (_remainingSeconds != 0)
                {
                    _remainingSeconds = 0;
                    OnChallengeTimeUpdate?.Invoke(_remainingSeconds);
                }

                AssignNewChallenges(true);
            }
        }

        public void UpdateChallenge(ChallengeType type, int amount)
        {
            foreach (var challenge in _currentChallenges)
            {
                if (challenge.Type != type) continue;
                challenge.Update(amount);
            }

            Save();
        }

        public void Save()
        {
            var binaryFormatter = new BinaryFormatter();
            if (_currentChallenges != null)
            {
                using var fileStream = File.Create(_savePath);
                binaryFormatter.Serialize(fileStream, _currentChallenges);
                fileStream.Close();
            }

            if (specialReward != null)
            {
                using var specialRewardFileStream = File.Create(_specialRewardSavePath);
                binaryFormatter.Serialize(specialRewardFileStream, specialReward);
                specialRewardFileStream.Close();
            }

            OnChallengeUpdate?.Invoke(_currentChallenges, specialReward);
        }

        private Challenge[] Load()
        {
            if (!File.Exists(_savePath)) return null;
            var binaryFormatter = new BinaryFormatter();
            using var fileStream = File.Open(_savePath, FileMode.Open);
            try
            {
                var challenges = (Challenge[]) binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                return challenges;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                fileStream.Close();
                return null;
            }
        }

        private SpecialReward LoadSpecialReward()
        {
            if (!File.Exists(_specialRewardSavePath)) return null;
            var binaryFormatter = new BinaryFormatter();
            using var fileStream = File.Open(_specialRewardSavePath, FileMode.Open);
            try
            {
                var reward = (SpecialReward) binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                return reward;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                fileStream.Close();
                return null;
            }
        }

        public void AssignNewSpecialReward()
        {
            if (specialReward != null && !specialReward.IsClaimed) return;
            specialReward = new SpecialReward()
            {
                Reward = new Reward()
                {
                    Price = new Price()
                    {
                        Type = CollectableType.Gem,
                        Amount = 20
                    }
                }
            };
            Save();
        }

        private void AssignNewChallenges(bool newDate)
        {
            var available = new List<Challenge>(AvailableChallenges);
            _currentChallenges = new Challenge[maxChallenges];
            for (var i = 0; i < maxChallenges; i++)
            {
                if (available.Count == 0)
                    available = new List<Challenge>(AvailableChallenges);
                if (available.Count == 0) break;
                _currentChallenges[i] = new Challenge(available[Random.Range(0, available.Count)]);
                for (var j = 0; j < available.Count; j++)
                {
                    if (available[j].Type != _currentChallenges[i].Type) continue;
                    available.RemoveAt(j);
                    j--;
                }
            }

            if (newDate)
                NextChallengeTime = DateTime.Today.AddDays(1);
            OnChallengeUpdate?.Invoke(_currentChallenges, specialReward);
            Save();
        }

        public void ResetData()
        {
            AssignNewChallenges(false);
        }

        public static Challenge[] GetCurrentChallenges()
        {
            return Instance._currentChallenges;
        }

#if UNITY_EDITOR
        // [MenuItem("Tools/Clear challenges")]
        public static void ClearChallenges()
        {
            Instance.NextChallengeTime = DateTime.Today;
        }
#endif

        private bool HasUnClaimed()
        {
            return _currentChallenges.Any(challenge => challenge.IsCompleted && !challenge.IsClaimed);
        }

        public static Sprite GetIcon(ChallengeType challengeType)
        {
            return null;
        }

        public static SpecialReward GetSpecialReward()
        {
            return Instance.specialReward;
        }
    }
}