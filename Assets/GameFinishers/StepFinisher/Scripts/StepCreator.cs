using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class StepCreator : MonoBehaviour
{
    [SerializeField] private GameObject stepPrefab;
    [SerializeField] private int stepCount;
    [SerializeField] private Vector3 stepOffset;
    public Transform[] Steps { get; private set; }

    private void Start()
    {
        CreateSteps();
    }

    [Button]
    private void CreateSteps()
    {
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        var offset = stepOffset;
        offset.y = 0;
        offset.z *= 0f;
        var property = new MaterialPropertyBlock();
        Steps = new Transform[stepCount];
        for (var i = 0; i < stepCount; i++)
        {
            var instance = Instantiate(stepPrefab, transform);
            instance.transform.localPosition = offset;
            var renderers = instance.GetComponentsInChildren<Renderer>();
            property.SetColor("_Color", Random.ColorHSV());
            foreach (var renderer in renderers)
            {
                renderer.SetPropertyBlock(property);
            }

            Steps[i] = instance.transform;

            offset += stepOffset;
        }
    }
}