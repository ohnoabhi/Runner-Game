using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running,
        Fall,
        Finish
    }

    public float Speed;
    public float SlideSpeed;
    public float XMovementClamp;

    [HideInInspector] public PlayerState State;

    public void Die()
    {
        ScreenController.instance.Show("Lose", 0, Array.Empty<object>());
    }

    public void Win()
    {
        ScreenController.instance.Show("Win", 0, Array.Empty<object>());
    }
}