using System;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private Obstacle obstacle;
    [SerializeField] private TextMeshPro text;

    private void Start()
    {
        text.text = obstacle.damage.ToString();
    }
}