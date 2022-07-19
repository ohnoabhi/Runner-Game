using System.Threading.Tasks;
using Challenges;
using Stats;
using UnityEngine;

public class BossFinisher : GameFinisher
{
    [SerializeField] private Boss[] bosses;
    [SerializeField] private Transform bossParent;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private GameObject ui;

    private bool start;

    private ProgressSlider playerHealth;
    // private ProgressSlider bossHealth;

    private float playerAttack = 1;
    private float bossAttack = 1;

    private BossFinisherUIController uiController;

    private float winPercentage = 0.5f;
    private Boss boss;

    protected override async void Finish()
    {
        base.Finish();


        boss = Instantiate(bosses[Random.Range(0, bosses.Length)], bossParent);
        // boss = Instantiate(bosses[5], bossParent);
        boss.transform.localRotation = Quaternion.Euler(0, 0, 0);
        var bossTask = boss.StartAttack(bossPosition.position);
        while (PlayerController.transform.position != playerPosition.position)
        {
            PlayerController.transform.position =
                Vector3.MoveTowards(PlayerController.transform.position, playerPosition.position, 7 * Time.deltaTime);
            await Task.Yield();
        }


        PlayerController.Animator.SetTrigger("Idle");
        await Task.WhenAll(bossTask);

        // PlayerController.Animator.SetTrigger("Attack");

        uiController = ScreenController.instance.SetFinisherUI(ui).GetComponent<BossFinisherUIController>();
        uiController.Init();
        GameManager.Instance.ShowFinisherUI();


        playerHealth = uiController.playerHealth;
        // bossHealth = uiController.bossHealth;
        winPercentage = 0.5f;
        playerHealth.SetValue(winPercentage);
        // bossHealth.SetValue(1 - winPercentage);

        playerAttack = 0.035f;

        // var playerLevel = StatsManager.Get(StatType.PlayerStat);
        // playerAttack += 15f * playerLevel / GameManager.Level;

        // bossAttack = (boss.Damage + (boss.Damage * (StatsManager.Get(StatType.PlayerStat) / 100f)) +
        //               (boss.Damage * (GameManager.Level / 100f)));
        bossAttack = 0.1f;
        // var bossLevel = GameManager.Level * 1.5f;
        // bossAttack *= (bossLevel / playerLevel);
        start = true;
    }

    private float delay;

    private void Update()
    {
        if (!start) return;

        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (winPercentage < 0.9f)
                winPercentage -= bossAttack * Time.deltaTime;
        }


        if (Input.GetMouseButtonDown(0))
        {
            delay = 0.2f;
            winPercentage += playerAttack;
            PlayerController.Animator.SetBool("Roaring", true);
        }
        else
        {
            PlayerController.Animator.SetBool("Roaring", false);
        }

        winPercentage = Mathf.Clamp(winPercentage, 0, 1);

        playerHealth.Value = winPercentage;
        // bossHealth.Value = 1 - winPercentage;
        CheckGameOver();
        // uiController.UpdateHealthUI(playerHealth, bossHealth);
    }

    private async void CheckGameOver()
    {
        if (playerHealth.Progress <= 0.05f)
        {
            start = false;
            await Task.Delay(1000);
            GameManager.Instance.GameOver(false);
        }
        else if (playerHealth.Progress >= 0.95f)
        {
            start = false;
            boss.Die();
            await Task.Delay(1500);
            ChallengeManager.Instance.UpdateChallenge(ChallengeType.KillBoss);
            GameManager.Instance.GameOver(true);
        }
    }
}