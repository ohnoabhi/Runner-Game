using System.Threading.Tasks;
using TMPro;
//using TopHud;
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
        [SerializeField] private Image iconPrefab;

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
                    btnClaim.onClick.AddListener(() => { SpawnIcon(challenge); });
                }
            }
        }

        private async Task SpawnIcon(Challenge challenge)
        {
            btnClaim.interactable = false;
            var target = TopHudController.instance.GetIconPos(CollectableType.Gem);

            var items = new Image[10];
            var delay = 100;
            for (var i = 0; i < 10; i++)
            {
                items[i] = Instantiate(iconPrefab, transform);
                items[i].transform.position = imgIcon.transform.position + new Vector3(-10, -10, 0);
                MoveCoin(items[i], target);
                delay -= 10;
                if (delay < 0) delay = 0;
                await Task.Delay(delay);
            }

            challenge.Claim();
        }

        private static async void MoveCoin(Image item, Vector3 target)
        {
            while (item.transform.position != target)
            {
                item.transform.position =
                    Vector3.MoveTowards(item.transform.position, target, 1500 * Time.deltaTime);
                await Task.Yield();
            }

            AudioManager.Play("GemCollect");
            Destroy(item);
        }
    }
}