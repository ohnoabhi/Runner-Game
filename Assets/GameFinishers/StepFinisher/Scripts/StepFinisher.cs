using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StepFinisher : GameFinisher
{
    [SerializeField] private StepCreator stepCreator;
    [SerializeField] private float playerMoveSpeed = 50f;

    protected override async void Finish()
    {
        base.Finish();

        var steps = stepCreator.Steps;

        var playerHealth = player.GetComponent<PlayerHealth>();
        var toReachPercentage = playerHealth.CurrentHealth / playerHealth.MaxHealth;

        var index = Mathf.FloorToInt(steps.Length * toReachPercentage);

        for (var i = 0; i < index; i++)
        {
            while (player.transform.position != steps[i].position)
            {
                player.transform.position =
                    Vector3.MoveTowards(player.transform.position, steps[i].position, playerMoveSpeed * Time.deltaTime);
                await Task.Yield();
            }
        }

        var target = player.transform.position += new Vector3(0, 0, 2.5f);
        while (player.transform.position != target)
        {
            player.transform.position =
                Vector3.MoveTowards(player.transform.position, target, playerMoveSpeed * Time.deltaTime);
            await Task.Yield();
            if (player.transform.position != target)
                player.transform.position += new Vector3(0, 0.5f, 0);
        }

        var character = player.GetComponent<PlayerCharacterManager>().Character;
        character.Animator.SetTrigger("Roar");
        await Task.Delay(1500);

        GameManager.Instance.GameOver(true);
    }
}