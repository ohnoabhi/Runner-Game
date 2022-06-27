using System.Collections.Generic;
using Challenges;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeSpecialRewardItemView : MonoBehaviour
{
    [SerializeField] private Button btnClaim;
    [SerializeField] private GameObject objClaim;
    [SerializeField] private GameObject objClaimed;
    [SerializeField] private List<GameObject> indicators;

    [SerializeField] private TextMeshProUGUI rewardAmount;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private GameObject reward;
    [SerializeField] private Image giftIcon;

    public void Init(SpecialReward challenge)
    {
        if (challenge.IsCompleted)
        {
            foreach (var item in indicators)
            {
                item.SetActive(true);
            }

            rewardAmount.text = challenge.Reward.Price.Amount.ToString();
            rewardIcon.sprite = challenge.Reward.Icon;
            objClaim.SetActive(!challenge.IsClaimed);

            reward.SetActive(!challenge.IsClaimed);
            giftIcon.gameObject.SetActive(challenge.IsClaimed);
            btnClaim.onClick.RemoveAllListeners();
            btnClaim.onClick.AddListener(() =>
            {
                AudioManager.OnButtonClick();
                ClaimAnimationManager.SpawnIcon(challenge.Reward.Price.Type, rewardIcon.transform.position,
                    rewardIcon.rectTransform.sizeDelta);
                challenge.Claim();
            });
            objClaimed.SetActive(challenge.IsClaimed);
        }
        else
        {
            objClaim.SetActive(false);
            objClaimed.SetActive(false);

            reward.gameObject.SetActive(false);
            giftIcon.gameObject.SetActive(true);
            for (var i = 0; i < indicators.Count; i++)
            {
                indicators[i].SetActive(i < challenge.Completed);
            }
        }
    }
}