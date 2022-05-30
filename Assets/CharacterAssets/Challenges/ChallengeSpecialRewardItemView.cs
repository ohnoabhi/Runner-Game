using System.Collections.Generic;
using System.Threading.Tasks;
using Challenges;
using TMPro;
//using TopHud;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeSpecialRewardItemView : MonoBehaviour
{
    [SerializeField] private GameObject objIndicator;
    [SerializeField] private Button btnClaim;
    [SerializeField] private GameObject objClaim;
    [SerializeField] private GameObject objClaimed;
    [SerializeField] private List<GameObject> indicators;

    [SerializeField] private TextMeshProUGUI rewardAmount;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private Image iconPrefab;

    public void Init(SpecialReward challenge)
    {
        if (challenge.IsCompleted)
        {
            rewardAmount.text = challenge.Reward.Price.Amount.ToString();
            rewardIcon.sprite = challenge.Reward.Icon;
            objIndicator.SetActive(false);
            objClaim.SetActive(!challenge.IsClaimed);
            btnClaim.onClick.RemoveAllListeners();
            btnClaim.onClick.AddListener(() => { SpawnIcon(challenge); });
            objClaimed.SetActive(challenge.IsClaimed);
        }
        else
        {
            objIndicator.SetActive(true);
            objClaim.SetActive(false);
            objClaimed.SetActive(false);

            for (var i = 0; i < indicators.Count; i++)
            {
                indicators[i].SetActive(i < challenge.Completed);
            }
        }
    }

    private async void SpawnIcon(SpecialReward challenge)
    {
        btnClaim.interactable = false;
        var target = TopHudController.instance.GetIconPos(CollectableType.Gem);

        var items = new Image[10];
        var delay = 100;
        for (var i = 0; i < 10; i++)
        {
            items[i] = Instantiate(iconPrefab, transform);
            items[i].transform.position = rewardIcon.transform.position + new Vector3(-10, -10, 0);
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