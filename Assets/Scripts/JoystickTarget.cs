using System;
using UnityEngine;
using UnityEngine.UI;

public class JoystickTarget : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float smoothness = 5;
    [SerializeField] private Text speedText;
    [SerializeField] private Text smoothnessText;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider smoothnessSlider;

    private void Start()
    {
        speedSlider.value = speed;
        smoothnessSlider.value = smoothness;
    }

    private void Update()
    {
        if (Utility.Input.TouchCount <= 0) return;
        var touch = Utility.Input.GetTouch(0);

        if (touch.phase != TouchPhase.Moved) return;

        var x = touch.deltaPosition.x;

        var movement = transform.position;
        movement.x += x * speed * Time.deltaTime;
        movement.x = Mathf.Clamp(movement.x, -2, 2);

        if (Vector3.Distance(transform.position, movement) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movement, smoothness * Time.deltaTime);
        }
    }

    public void OnSpeedChange(float speed)
    {
        this.speed = speed;
        speedText.text = speed.ToString();
    }

    public void OnSmoothnessChange(float smoothness)
    {
        this.smoothness = smoothness;
        smoothnessText.text = smoothness.ToString();
    }

    public void SetTarget(float x)
    {
    }
}