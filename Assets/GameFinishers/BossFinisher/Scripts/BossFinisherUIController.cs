using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossFinisherUIController : MonoBehaviour
{
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider bossHealth;
    [SerializeField] private RectTransform tap;
    [SerializeField] private TextMeshProUGUI playerName;

    public void Init()
    {
        playerHealth.value = 1;
        bossHealth.value = 1;
        playerName.text = Username.Name;
        tap.DOScale(1.5f, 0.5f).SetLoops(-1);
    }

    public void UpdateHealthUI(float player, float boss)
    {
        playerHealth.value = player;
        bossHealth.value = boss;
    }
}