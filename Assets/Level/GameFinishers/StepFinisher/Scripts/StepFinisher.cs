using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StepFinisher : GameFinisher
{
    [SerializeField] private StepCreator stepCreator;
    [SerializeField] private float playerMoveSpeed = 50f;
    [SerializeField] private int stepDamage = 20;

    protected override async void Finish()
    {
        base.Finish();

        var steps = stepCreator.Steps;

        var stepsReached = 0;
        for (var i = 0; i < steps.Length; i++)
        {
            var position = steps[i].transform.position + new Vector3(0, 0f, 2.5f);
            while (PlayerController.transform.position != position)
            {
                PlayerController.transform.position =
                    Vector3.MoveTowards(PlayerController.transform.position, position,
                        playerMoveSpeed * Time.deltaTime);
                await Task.Yield();
            }

            AudioManager.Play("ConfettiBlast");
            foreach (var particle in steps[i].GetParticles())
            {
                particle.Play();
            }

            PlayerController.TakeDamage(stepDamage);
            stepsReached++;
            if (PlayerController.Health <= stepDamage) break;

            position += new Vector3(0, 0f, 2.5f);
            while (PlayerController.transform.position != position)
            {
                PlayerController.transform.position =
                    Vector3.MoveTowards(PlayerController.transform.position, position,
                        playerMoveSpeed * Time.deltaTime);
                await Task.Yield();
            }

            position += new Vector3(0, 0.5f, 0);
            while (PlayerController.transform.position != position)
            {
                PlayerController.transform.position =
                    Vector3.MoveTowards(PlayerController.transform.position, position,
                        playerMoveSpeed * Time.deltaTime);
                await Task.Yield();
            }
        }

        PlayerController.Animator.SetTrigger("Idle");
        await Task.Delay(1500);

        GameManager.Instance.GameOver(true, 0,stepsReached);
    }
}