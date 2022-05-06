using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public float Speed;
    public float SlideSpeed;
    public float XMovementClamp;

    public int CurrentHealth;

    public Action<int> OnChangeHealthLevel;

    private PlayerHealth health;
    public PlayerHealth Health => health;

    private void Awake()
    {
        Instance = this;

        health = GetComponent<PlayerHealth>();
    }

    public void GainHealth(int amount = 1) => Health?.GainHealth(amount);
    public void TakeDamage(int amount = 1) => Health?.TakeDamage(amount);

    public void Die()
    {
        UIScreenController.instance.Show("Lose", 0, Array.Empty<object>());
    }

    public void Win()
    {
        UIScreenController.instance.Show("Win", 0, Array.Empty<object>());
    }
}