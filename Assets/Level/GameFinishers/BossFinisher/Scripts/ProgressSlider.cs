using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    public float Value;

    private float Speed = 5;
    public float Min = 0;
    public float Max = 1;
    private Slider slider;
    public float Progress => slider.value;

    public void SetValue(float value)
    {
        Value = Mathf.Clamp(value, Min, Max);
        slider.value = value;
    }

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (slider.value != Value)
        {
            slider.value = Mathf.Lerp(slider.value, Value, Speed * Time.deltaTime);
        }
    }
}