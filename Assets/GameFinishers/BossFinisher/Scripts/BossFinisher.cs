using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossFinisher : GameFinisher
{
    [SerializeField] private Boss boss;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private GameObject ui;

    private bool start = false;

    private float playerHealth = 1;
    private float bossHealth = 1;

    private BossFinisherUIController uiController;

    protected override async void Finish()
    {
        base.Finish();
        playerHealth = 1;
        bossHealth = 1;

        boss.StartAttack(bossPosition.position, player);
        while (player.transform.position != playerPosition.position)
        {
            player.transform.position =
                Vector3.MoveTowards(player.transform.position, playerPosition.position, 5 * Time.deltaTime);
            await Task.Yield();
        }

        var character = player.GetComponent<PlayerCharacterManager>().Character;
        if (character)
            character.Animator.SetBool("IsAttacking", true);

        uiController = ScreenController.instance.SetFinisherUI(ui).GetComponent<BossFinisherUIController>();
        uiController.Init();
        GameManager.Instance.ShowFinisherUI();
        start = true;
    }

    private void Update()
    {
        if (start)
        {
            playerHealth -= boss.Damage * Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                bossHealth -= 2f * Time.deltaTime;
            }

            if (playerHealth < 0) playerHealth = 0;
            if (bossHealth < 0) bossHealth = 0;

            if (playerHealth == 0) GameManager.Instance.GameOver(false);
            else if (bossHealth == 0) GameManager.Instance.GameOver(true);

            uiController.UpdateHealthUI(playerHealth, bossHealth);
        }
    }
}