using UnityEngine;

public class VibrationManager : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static bool VibrationOn
    {
        get => PlayerPrefs.GetInt("Vibration", 1) == 1;
        set => PlayerPrefs.SetInt("Vibration", value ? 1 : 0);
    }

    public static bool isAndroid
    {
        get
        {
            // #if UNITY_ANDROID && !UNITY_EDITOR
            //             return true;
            // #else
            //             return false;
            // #endif

            return Application.platform == RuntimePlatform.Android;
        }
    }

    public static void Vibrate(long milliseconds = 250)
    {
#if UNITY_EDITOR
        return;
#endif
#pragma warning disable 162
        if (!VibrationOn) return;
        if (!isAndroid)
        {
            Handheld.Vibrate();
            return;
        }

        vibrator.Call("vibrate", milliseconds);
#pragma warning restore 162
    }

    public static void Cancel()
    {
        if (!isAndroid)
        {
            return;
        }

        vibrator.Call("cancel");
    }
}