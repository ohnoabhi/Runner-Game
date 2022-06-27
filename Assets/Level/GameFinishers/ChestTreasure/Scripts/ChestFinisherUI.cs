using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChestFinisherUI : MonoBehaviour
{
    [SerializeField] private ProgressSlider slider;
    [SerializeField] private RectTransform tap;

    public void Init()
    {
        slider.SetValue(0);
        tap.DOScale(1.5f, 0.5f).SetLoops(-1);
    }

    public void UpdateValue(float value)
    {
        slider.Value = value;
    }
}