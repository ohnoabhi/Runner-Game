using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ChestFinisher : GameFinisher
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform chestParent;
    [SerializeField] private GameObject ui;
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private ParticleSystem cashParticles;
    private ChestFinisherUI uiController;
    private bool start;
    float value;

    protected override async void Finish()
    {
        base.Finish();

        var playerTransform = PlayerController.transform;
        var playerSize = PlayerController.GetSize();

        var position = playerPosition.localPosition;
        position.z -= playerSize.x * 1.2f;
        if (position.z < 1)
        {
            position.z = 1;
        }

        playerPosition.localPosition = position;

        chestParent.localScale = Vector3.one * playerSize.x;

        while (PlayerController && playerTransform.position != playerPosition.position)
        {
            playerTransform.position =
                Vector3.MoveTowards(playerTransform.position, playerPosition.position, 6 * Time.deltaTime);
            await Task.Yield();
        }

        PlayerController.Animator.SetTrigger("Idle");

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
            value += 0.08f;
            PlayerController.Animator.SetBool("Roaring", true);
        }
        else
        {
            PlayerController.Animator.SetBool("Roaring", false);
        }


        value -= 0.15f * Time.deltaTime;
        value = Mathf.Clamp(value, 0, 1);
        uiController.UpdateValue(value);
        if (value >= 1)
            GameOver();
    }

    private async void GameOver()
    {
        start = false;
        uiController.UpdateValue(1);
        PlayerController.Animator.SetBool("Roaring", false);
        chestAnimator.SetTrigger("Open");
        AudioManager.Play("OpenChest");
        await Task.Delay(250);
        cashParticles.Play();
        await Task.Delay(1500);
        GameManager.Instance.GameOver(true, 0, 1, true);
    }
}