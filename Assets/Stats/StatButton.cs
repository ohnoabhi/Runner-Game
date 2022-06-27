using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stats
{
    public class StatButton : Button
    {
        [SerializeField] private StatType type;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Image priceIcon;

        protected override void Start()
        {
            base.Start();

            if (!Application.isPlaying) return;
            UpdateUI();
            priceIcon.sprite = CollectablesManager.GetIcon(StatsManager.GetPrice(type).Type);

            onClick.RemoveAllListeners();
            onClick.AddListener(OnClick);

            StatsManager.OnUpdate += UpdateUI;
            
            CollectablesManager.RegisterForUpdate(OnCollectableUpdate);
        }

        private void OnCollectableUpdate(Price obj)
        {
            UpdateUI();
        }

        private void OnClick()
        {
            AudioManager.OnButtonClick();
            var price = StatsManager.GetPrice(type);
            if (!price.IsAffordable()) return;
            price.Pay();
            StatsManager.Update(type);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StatsManager.OnUpdate -= UpdateUI;
            CollectablesManager.DeRegisterForUpdate(OnCollectableUpdate);
        }

        private void UpdateUI()
        {
            progressText.text = "LVL " + StatsManager.Get(type);
            var price = StatsManager.GetPrice(type);
            interactable = price.IsAffordable();
            priceText.text = price.Amount.ToString();
        }
    }
}