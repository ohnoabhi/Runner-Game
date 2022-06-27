using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Settings;
using Sirenix.OdinInspector;

public class SettingsController : MonoBehaviour
{
    [BoxGroup] [SerializeField] private float animationSpeed;
    [BoxGroup] [SerializeField] private float animationDelay;
    [SerializeField] SettingsButton[] SettingsButtons;

    private bool isShowing;

    private bool animationRunning;

    private void Awake()
    {
        foreach (var button in SettingsButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { OnClickButton(button); });
            SetButton(button);
        }

        isShowing = false;
        Hide(true);
    }

    private void ToggleButton(SettingsButton button)
    {
        switch (button.id)
        {
            case "Sound":
                AudioManager.SoundON = !AudioManager.SoundON;
                break;

            case "Haptic":
                VibrationManager.VibrationOn = !VibrationManager.VibrationOn;
                break;
        }

        SetButton(button);
    }

    private void SetButton(SettingsButton button)
    {
        switch (button.id)
        {
            case "Sound":
                button.Enabled = AudioManager.SoundON;
                break;

            case "Haptic":
                button.enabled = VibrationManager.VibrationOn;
                break;
        }
    }

    private void OnClickButton(SettingsButton button)
    {
        AudioManager.OnButtonClick();
        ToggleButton(button);
    }

    public void TogglePanel()
    {
        if (animationRunning) return;

        if (isShowing)
        {
            Hide(false);
        }
        else
        {
            Show();
        }
    }

    private async void Hide(bool immediate)
    {
        animationRunning = true;
        var delay = 0f;

        for (var i = SettingsButtons.Length - 1; i >= 0; i--)
        {
            var settingsButton = SettingsButtons[i];
            ((RectTransform) settingsButton.transform).DOAnchorPosX(-175, immediate ? 0 : animationSpeed)
                .SetDelay(immediate ? 0 : delay);
            delay += animationDelay;
        }

        await Task.Delay(
            immediate
                ? 0
                : (int) ((animationSpeed + animationDelay * (SettingsButtons.Length - 1)) *
                         1000));
        animationRunning = false;
        isShowing = false;
        isShowing = false;
    }

    private async void Show()
    {
        animationRunning = true;
        var delay = 0f;
        foreach (var settingsButton in SettingsButtons)
        {
            ((RectTransform) settingsButton.transform).DOAnchorPosX(75, animationSpeed).SetDelay(delay);
            delay += animationDelay;
        }

        await Task.Delay(
            (int) ((animationSpeed + animationDelay * (SettingsButtons.Length - 1)) *
                   1000));
        animationRunning = false;
        isShowing = true;
    }
}