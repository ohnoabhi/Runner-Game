using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFinisherUIController : MonoBehaviour
{
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider bossHealth;

    public void Init()
    {
        playerHealth.value = 1;
        bossHealth.value = 1;
    }

    public void UpdateHealthUI(float player, float boss)
    {
        playerHealth.value = player;
        bossHealth.value = boss;
    }
}