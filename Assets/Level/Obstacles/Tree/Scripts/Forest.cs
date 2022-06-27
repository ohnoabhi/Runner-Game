using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Forest : MonoBehaviour
{
    private void OnEnable()
    {
        RotateChildren();
        RandomSize();
    }

    [Button]
    private void RotateChildren()
    {
        foreach (Transform child in transform)
        {
            child.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }

    [Button]
    private void RandomSize()
    {
        foreach (Transform child in transform)
        {
            child.localScale = Vector3.one * Random.Range(0.75f, 1f);
        }
    }

    private bool soundPlayed;

    public void PlaySound()
    {
        if (soundPlayed) return;
        soundPlayed = true;
        AudioManager.Play("TreeFall");
    }

    public void ResetTrees()
    {
        soundPlayed = false;
        foreach (Transform child in transform)
        {
            child.GetComponent<Tree>().ResetObstacle();
        }
    }
}