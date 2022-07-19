using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameScreen : BaseScreen
{
    [SerializeField] private GameObject tutorial;
    [SerializeField] private TextMeshProUGUI cashText;

    protected override async void OnShow(params object[] args)
    {
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

    public void UpdateCash(int amount)
    {
        cashText.text = amount.ToString();
    }
}