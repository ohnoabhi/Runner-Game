using UnityEngine;

public static class Username
{
    public static string Name
    {
        get => PlayerPrefs.GetString("Username", "");
        set => PlayerPrefs.SetString("Username", value);
    }
}

public class UsernameController : MonoBehaviour
{
    private void Start()
    {
        if (string.IsNullOrEmpty(Username.Name))
        {
            PopupController.instance.Show("Username");
        }
    }
}