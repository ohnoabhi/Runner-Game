using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardAmount;
    private Price winPrice;
    protected override void OnShow(params object[] args)
    {
        winPrice = (Price) args[0];

        rewardIcon.sprite = CollectablesManager.GetIcon(winPrice.Type);
        rewardAmount.text = winPrice.Amount.ToString();
    }

    public void OnClickContinue()
    {
        winPrice.Gain();
        ScreenController.instance.Show("Menu");
    }
}