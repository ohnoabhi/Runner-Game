using System;
using Challenges;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseScreen
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider levelProgress;
    [SerializeField] private Slider bossLevelProgress;
    [SerializeField] private Slider chestLevelProgress;
    [SerializeField] private string minigameSceneName = "MiniGame";

    [SerializeField] private GameObject challengeNotify;
    public Slider speedSlider;
    public Slider smoothnessSlider;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI smoothnessText;

    private void OnEnable()
    {
        ChallengeManager.OnChallengeUpdate += OnChallengeUpdate;
    }

    private void OnDisable()
    {
        ChallengeManager.OnChallengeUpdate -= OnChallengeUpdate;
    }

    private void OnChallengeUpdate(Challenge[] arg1, SpecialReward arg2)
    {
        challengeNotify.SetActive(ChallengeManager.Instance.HasUnClaimed());
    }

    protected override void OnShow(params object[] args)
    {
        levelText.text = "Level " + GameManager.Level;

        challengeNotify.SetActive(ChallengeManager.Instance.HasUnClaimed());
        if (ChallengeManager.Instance.HasUnClaimed())
        {
            PopupController.instance.Show("Challenge");
        }

        const int count = 10;
        levelProgress.value = GameManager.Level % count;
        var levelPercentage = Mathf.FloorToInt(GameManager.Level / (float) count);
        var start = (levelPercentage * count);
        var hasBoss = false;
        var hasChest = false;
        var levels = LevelDatabase.Get().Levels;
        for (var i = start; i < Mathf.Min(start + (count - 1), levels.Count); i++)
        {
            if (!hasBoss && levels[i].EndType == LevelEndType.BossMatch)
            {
                hasBoss = true;
                bossLevelProgress.value = i + 1 - start;
            }

            if (!hasChest && levels[i].EndType == LevelEndType.Chest)
            {
                hasChest = true;
                chestLevelProgress.value = i + 1 - start;
            }
        }

        bossLevelProgress.gameObject.SetActive(hasBoss);
        chestLevelProgress.gameObject.SetActive(hasChest);

        speedSlider.value = PlayerController.PlayerSpeed;
        smoothnessSlider.value = PlayerController.PlayerSlideSpeed;
    }

    public void OnClickPlay()
    {
        GameManager.Instance.StartGame();
    }

    public void OnClickMiniGame()
    {
        PopupController.instance.Show("Loading");
        SceneManager.LoadScene(minigameSceneName);
    }

    public void OnPlayerSpeedChange(float value)
    {
        PlayerController.PlayerSpeed = (int) value;
        speedText.text = value.ToString();
    }

    public void OnPlayerSmoothnessChange(float value)
    {
        PlayerController.PlayerSlideSpeed = (int) value;
        smoothnessText.text = value.ToString();
    }
}