using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float SlideSpeed;
    public float XMovementClamp;

    public void Die()
    {
        ScreenController.instance.Show("Lose", 0, Array.Empty<object>());
    }

    public void Win()
    {
        ScreenController.instance.Show("Win", 0, Array.Empty<object>());
    }
}