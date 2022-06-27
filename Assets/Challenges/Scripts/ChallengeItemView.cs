using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Challenges
{
    public class ChallengeItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private Image imgIcon;
        [SerializeField] private Slider sldProgress;
        [SerializeField] private TextMeshProUGUI txtProgress;
        [SerializeField] private Button btnClaim;
        [SerializeField] private Image imgReward;
        [SerializeField] private TextMeshProUGUI txtReward;
        [SerializeField] private GameObject objClaim;
        [SerializeField] private GameObject objNotify;
        [SerializeField] private Image background;
        [SerializeField] private Color normal;
        [SerializeField] private Color claim;
        [SerializeField] private Color claimed;

        public void Init(Challenge challenge)
        {
            txtTitle.text = challenge.Type.GetDescription(challenge.Required);
            imgIcon.sprite = challenge.Type.GetIcon();
            sldProgress.value = (float) challenge.Completed / challenge.Required;
            if (txtProgress)
                txtProgress.text = challenge.Completed + "/" + challenge.Required;
            imgReward.sprite = challenge.Reward.Icon;
            txtReward.text = challenge.Reward.Price.Amount.ToString();
            if (challenge.IsCompleted && challenge.IsClaimed)
            {
                objNotify.SetActive(false);
                objClaim.SetActive(true);
                btnClaim.gameObject.SetActive(false);
                sldProgress.gameObject.SetActive(false);
                background.color = claimed;
            }
            else
            {
                objClaim.SetActive(false);
                btnClaim.gameObject.SetActive(challenge.IsCompleted && !challenge.IsClaimed);
                sldProgress.gameObject.SetActive(!(challenge.IsCompleted && !challenge.IsClaimed));
                btnClaim.interactable = challenge.IsCompleted && !challenge.IsClaimed;
                objNotify.SetActive(challenge.IsCompleted && !challenge.IsClaimed);
                background.color = challenge.IsCompleted && !challenge.IsClaimed ? claim : normal;
                if (challenge.IsCompleted && !challenge.IsClaimed)
                {
                    btnClaim.onClick.RemoveAllListeners();
                    btnClaim.onClick.AddListener(() =>
                    {
                        AudioManager.OnButtonClick();
                        ClaimAnimationManager.SpawnIcon(challenge.Reward.Price.Type, imgIcon.transform.position,
                            imgIcon.rectTransform.sizeDelta);
                        challenge.Claim();
                    });
                }
            }
        }
    }
}