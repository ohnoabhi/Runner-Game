using System;
using UnityEngine;

public class BossWall : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Boss"))
        {
            triggered = true;
            PlaySound();
        }
    }

    public void PlaySound()
    {
        AudioManager.Play("WallBreak");
    }
}