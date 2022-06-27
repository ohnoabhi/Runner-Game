using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BossFinisherUIController : MonoBehaviour
{
    [SerializeField] public ProgressSlider playerHealth;
    [SerializeField] public ProgressSlider bossHealth;
    [SerializeField] private RectTransform tap;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameObject tutorial;

    public async void Init()
    {
        playerName.text = Username.Name;
        tap.DOScale(1.5f, 0.5f).SetLoops(-1);

        if (GameManager.Level == 1)
        {
            tutorial.SetActive(true);
            await Task.Delay(2000);
            tutorial.SetActive(false);
        }
        else
        {
            tutorial.SetActive(false);
        }
    }
}