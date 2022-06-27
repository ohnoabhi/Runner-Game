using System.Threading.Tasks;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    public async void Show(int delay = 0, params object[] args)
    {
        await Task.Delay(delay);
        gameObject.SetActive(true);
        OnShow(args);
    }

    public void Hide()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    protected virtual void OnShow(params object[] args)
    {
    }

    protected virtual void OnHide()
    {
    }
}