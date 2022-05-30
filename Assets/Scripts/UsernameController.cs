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
        var isNullOrEmpty = string.IsNullOrEmpty(Username.Name);
        if (isNullOrEmpty)
        {
            PopupController.instance.Show("Username");
        }
    }
}