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


        while (player.transform.position != playerPosition.position)
        {
            player.transform.position =
                Vector3.MoveTowards(player.transform.position, playerPosition.position, 3 * Time.deltaTime);
            await Task.Yield();
        }

        var character = player.GetComponent<PlayerCharacterManager>().Character;
        character.Animator.SetTrigger("Roar");

        uiController = ScreenController.instance.SetFinisherUI(ui).GetComponent<SoldierFinisherUI>();
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
}