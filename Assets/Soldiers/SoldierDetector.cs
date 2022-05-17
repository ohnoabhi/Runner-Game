using System;
using System.Collections;
using System.Collections.Generic;
using Soldiers;
using UnityEngine;

public class SoldierDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (Soldier.Get(other, out var soldier))
        {
            soldier.Die();
        }
    }
}