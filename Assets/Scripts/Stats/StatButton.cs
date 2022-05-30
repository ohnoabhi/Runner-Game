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

        protected override void Start()
        {
            base.Start();

            if (!Application.isPlaying) return;
            UpdateUI();

            onClick.RemoveAllListeners();
            onClick.AddListener(() =>
            {
                var price = StatsManager.GetPrice(type);
                if (!price.IsAffordable()) return;
                price.Pay();
                StatsManager.Update(type);
            });

            StatsManager.OnUpdate += UpdateUI;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StatsManager.OnUpdate -= UpdateUI;
        }

        private void UpdateUI()
        {
            progressText.text = StatsManager.Get(type).ToString();
            var price = StatsManager.GetPrice(type);
            interactable = price.IsAffordable();
            priceText.text = price.Amount.ToString();
        }
    }
}