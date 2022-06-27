//using Panel;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Challenges
{
    public class ChallengePanel : BasePopup
    {
        [SerializeField] private ChallengeItemView[] _itemViews;
        [SerializeField] private ChallengeSpecialRewardItemView specialRewardItemView;
        [SerializeField] private TextMeshProUGUI txtTime;
        [SerializeField] private GameObject objReset;
        private GameObject objTxtTimeParent;

        private void Start()
        {
            PopulateUI(ChallengeManager.GetCurrentChallenges(), ChallengeManager.GetSpecialReward());

            ChallengeManager.OnChallengeUpdate += PopulateUI;
            ChallengeManager.OnChallengeTimeUpdate += OnTimeUpdate;
        }

        private void OnDestroy()
        {
            ChallengeManager.OnChallengeUpdate -= PopulateUI;
        }

        private void OnTimeUpdate(double remainingTime)
        {
            //if (!IsShowing) return;

            if (objTxtTimeParent == null)
                objTxtTimeParent = txtTime.transform.parent.gameObject;
            if (remainingTime > 0)
            {
                if (!objTxtTimeParent.activeSelf)
                    objTxtTimeParent.SetActive(true);
                txtTime.text = ToTimeString(remainingTime);
            }
            else
            {
                if (objTxtTimeParent.activeSelf)
                    objTxtTimeParent.SetActive(false);
            }
        }


        private static string ToTimeString(double seconds)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(seconds);
            return $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";
        }

        private void PopulateUI(Challenge[] challenges, SpecialReward specialReward)
        {
            objReset.SetActive(false);

            var allClaimed = true;
            if (challenges != null)
            {
                for (var i = 0; i < challenges.Length; i++)
                {
                    if (!challenges[i].IsClaimed) allClaimed = false;

                    _itemViews[i].Init(challenges[i]);
                }
            }

            specialRewardItemView.Init(specialReward);

            objReset.SetActive(allClaimed);
        }


        /* protected override void OnShow()
         {
         }
 
         protected override void OnHide()
         {
         }*/
    }
}