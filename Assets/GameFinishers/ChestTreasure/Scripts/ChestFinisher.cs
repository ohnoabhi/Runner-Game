using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ChestFinisher : GameFinisher
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject ui;
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private ParticleSystem cashParticles;
    private ChestFinisherUI uiController;
    private bool start;
    float value;

    protected override async void Finish()
    {
        base.Finish();


        while (player && player.transform.position != playerPosition.position)
        {
            player.transform.position =
                Vector3.MoveTowards(player.transform.position, playerPosition.position, 3 * Time.deltaTime);
            await Task.Yield();
        }

        var character = player.GetComponent<PlayerCharacterManager>().Character;
        character.Animator.SetTrigger("Roar");

        uiController = ScreenController.instance.SetFinisherUI(ui).GetComponent<ChestFinisherUI>();
        uiController.Init();
        value = 0;
        start = true;
    }

    private void Update()
    {
        if (!start) return;
        if (Input.GetMouseButtonDown(0))
        {
            value += 0.1f;
        }

        value -= 0.02f * Time.deltaTime;
        value = Mathf.Clamp(value, 0, 1);
        uiController.UpdateValue(value);
        if (value >= 1)
            GameOver();
    }

    private async void GameOver()
    {
        start = false;
        uiController.UpdateValue(1);
        player.GetComponent<PlayerCharacterManager>().Character.Animator.SetTrigger("Idle");
        chestAnimator.SetTrigger("Open");
        await Task.Delay(250);
        cashParticles.Play();
        await Task.Delay(1500);
        GameManager.Instance.GameOver(true);
    }
}