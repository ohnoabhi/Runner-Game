using System.Threading.Tasks;
using Stats;
using UnityEngine;

public class BossFinisher : GameFinisher
{
    [SerializeField] private Boss[] bosses;
    [SerializeField] private Transform bossParent;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private GameObject ui;

    private bool start = false;

    private float playerHealth = 1;
    private float bossHealth = 1;

    private float playerAttack = 1;
    private float bossAttack = 1;

    private BossFinisherUIController uiController;

    protected override async void Finish()
    {
        base.Finish();
        playerHealth = 1;
        bossHealth = 1;

        var boss = Instantiate(bosses[Random.Range(0, bosses.Length)], bossParent);
        boss.transform.localRotation = Quaternion.Euler(0, 0, 0);
        var bossTask = boss.StartAttack(bossPosition.position, player);
        while (player.transform.position != playerPosition.position)
        {
            player.transform.position =
                Vector3.MoveTowards(player.transform.position, playerPosition.position, 5 * Time.deltaTime);
            await Task.Yield();
        }

        var character = player.GetComponent<PlayerCharacterManager>().Character;
        if (character)
            character.Animator.SetBool("Running", false);

        await Task.WhenAll(bossTask);

        if (character)
            character.Animator.SetBool("IsAttacking", true);

        uiController = ScreenController.instance.SetFinisherUI(ui).GetComponent<BossFinisherUIController>();
        uiController.Init();
        GameManager.Instance.ShowFinisherUI();

        var currentHealthPercentage = GameManager.Instance.player.GetComponent<PlayerHealth>().CurrentHealth / 100f;
        playerAttack = currentHealthPercentage * (2 + (2 * (StatsManager.Get(StatType.PlayerStat) / 100f)));

        // bossAttack = (boss.Damage + (boss.Damage * (StatsManager.Get(StatType.PlayerStat) / 100f)) +
        //               (boss.Damage * (GameManager.Level / 100f)));
        bossAttack = 0.15f;
        var bossLevel = GameManager.Level * 1.5f;
        var per = StatsManager.Get(StatType.PlayerStat) / bossLevel;
        playerAttack = 2f * per;
        playerAttack -= playerAttack * 0.3f * (1 - currentHealthPercentage);
        start = true;
    }

    private void Update()
    {
        if (start)
        {
            playerHealth -= bossAttack * Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                bossHealth -= playerAttack * Time.deltaTime;
            }

            if (playerHealth < 0) playerHealth = 0;
            if (bossHealth < 0) bossHealth = 0;

            if (playerHealth == 0) GameManager.Instance.GameOver(false);
            else if (bossHealth == 0) GameManager.Instance.GameOver(true);

            uiController.UpdateHealthUI(playerHealth, bossHealth);
        }
    }
}