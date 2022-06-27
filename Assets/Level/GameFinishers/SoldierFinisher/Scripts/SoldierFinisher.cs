using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierFinisher : GameFinisher
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private SoldierRunner[] soldiers;
    [SerializeField] private GameObject ui;

    private SoldierFinisherUI uiController;
    private bool start = false;
    float value = 0;

    protected override async void Finish()
    {
        base.Finish();
        foreach (var soldier in soldiers)
        {
            soldier.SetCanShoot(false);
        }

        while (PlayerController && PlayerController.transform.position != playerPosition.position)
        {
            PlayerController.transform.position =
                Vector3.MoveTowards(PlayerController.transform.position, playerPosition.position, 6 * Time.deltaTime);
            await Task.Yield();
        }

        foreach (var soldier in soldiers)
        {
            soldier.SetCanShoot(true);
        }

        if (PlayerController.State != PlayerController.PlayerState.Dead)
            PlayerController.Animator.SetTrigger("Idle");


        uiController = ScreenController.instance.SetFinisherUI(ui).GetComponent<SoldierFinisherUI>();
        uiController.Init();
        value = 0.15f;
        start = true;
    }

    private void Update()
    {
        if (!start) return;

        if (PlayerController.State == PlayerController.PlayerState.Dead)
        {
            foreach (var soldier in soldiers)
            {
                soldier.SetCanShoot(false);
            }

            GameOver(false);
        }

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
            GameOver(true);
        // else if (value <= 0)
        //     GameOver(false);
    }

    private async void GameOver(bool win)
    {
        start = false;


        foreach (var soldier in soldiers)
        {
            soldier.SetCanShoot(false);
        }

        if (win)
        {
            PlayerController.Animator.SetBool("Roaring", false);
            uiController.UpdateValue(1);
            var targets = new Vector3[waypoints.Length];
            var i = 0;
            foreach (var waypoint in waypoints)
            {
                targets[i] = waypoint.position;
                i++;
            }

            var tasks = soldiers.Select(soldier => soldier.Run(targets)).ToList();

            await Task.WhenAll(tasks);
            GameManager.Instance.GameOver(true);
        }
        else
        {
            uiController.UpdateValue(0);
            PlayerController.Animator.SetTrigger("Dead");
            await Task.Delay(1500);
            GameManager.Instance.GameOver(false);
        }
    }
}