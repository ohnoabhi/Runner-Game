using System;
using System.Threading.Tasks;
using DG.Tweening;
using EasyUI.PickerWheelUI;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions.CasualGame;

public class WinScreen : BaseScreen
{
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardAmount;

    [SerializeField] private RectTransform noThanksButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private GameObject adIndicator;
    [SerializeField] private PickerWheel pickerWheel;
    private Price winPrice;
    private int multiplier;
    private bool isChest;
    private bool spinned;

    protected override void OnShow(params object[] args)
    {
        AudioManager.Play("Win");
        winPrice = (Price) args[0];
        multiplier = (int) args[1];
        isChest = (bool) args[2];

        winPrice.Amount *= multiplier;
        if (isChest)
        {
            var chestAmount = Mathf.RoundToInt(20 + (20 * (0.25f * (StatsManager.Get(StatType.RewardMultiplier) - 1))));
            winPrice.Amount += chestAmount;
        }

        continueText.text = "Claim";
        adIndicator.SetActive(true);
        noThanksButton.gameObject.SetActive(true);
        noThanksButton.DOAnchorPosY(-638, 0);
        noThanksButton.DOAnchorPosY(-838, 1).SetDelay(2);

        rewardIcon.sprite = CollectablesManager.GetIcon(winPrice.Type);
        rewardAmount.text = winPrice.Amount.ToString();
    }


    public void OnClickContinue()
    {
        if (spinned)
        {
            OnClickNoThanks();
        }
        else
        {
            spinned = true;
            noThanksButton.gameObject.SetActive(false);
            continueText.text = "Continue";
            adIndicator.SetActive(false);
            continueButton.interactable = false;
            pickerWheel.Spin((x) =>
            {
                continueButton.interactable = true;
                winPrice.Amount *= x;
                rewardAmount.text = winPrice.Amount.ToString();
                rewardAmount.transform.DOShakeScale(.3f, 1f, 1);
            });
        }
    }

    public async void OnClickNoThanks()
    {
        await ClaimAnimationManager.SpawnIcon(winPrice.Type, rewardIcon.transform.position,
            rewardIcon.rectTransform.sizeDelta);
        winPrice.Gain();
        ScreenController.instance.Show("Menu");
    }
}