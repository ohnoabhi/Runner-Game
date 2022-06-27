using System.Threading.Tasks;
using UnityEngine;

public class GameScreen : BaseScreen
{
    [SerializeField] private GameObject tutorial;

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
}