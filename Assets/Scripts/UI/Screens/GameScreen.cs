using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : BaseScreen
{
    [SerializeField] TextMeshProUGUI levelNo;

    [SerializeField] Slider healthMeter;

    private void OnEnable()
    {
        GameManager.OnPlayerHealthChange += SetPlayerHealth;
        GameManager.OnPlayerHealthLevelChange += SetPlayerHealthLevel;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerHealthChange -= SetPlayerHealth;
        GameManager.OnPlayerHealthLevelChange -= SetPlayerHealthLevel;
    }

    private void SetPlayerHealthLevel(int level)
    {
        levelNo.text = level.ToString();
    }

    private void SetPlayerHealth(float health, int minHealth, int maxHealth)
    {
        healthMeter.minValue = maxHealth;
        healthMeter.value = health;
        healthMeter.maxValue = maxHealth;
    }
}