using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Joystick : MonoBehaviour
{
    [SerializeField] private RectTransform hodler;
    [SerializeField] private RectTransform handle;

    [SerializeField] private JoystickTarget target;
    private bool mouseDown;
    public Action<Vector2> OnSwipe;

    private float maxX = 2;

    private void Awake()
    {
        hodler.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
            hodler.position = Input.mousePosition;
            hodler.gameObject.SetActive(true);
            handle.gameObject.SetActive(true);
        }

        if (mouseDown)
        {
            if (Mathf.Abs(target.transform.position.x) >= maxX)
            {
                hodler.position = handle.position;
            }
            else
            {
                handle.position = Input.mousePosition;
            }

            var x = handle.position.x - hodler.position.x;
            target.SetTarget(x);
            OnSwipe?.Invoke(handle.position - hodler.position);
        }
        else
        {
            OnSwipe?.Invoke(Vector2.zero);
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            hodler.gameObject.SetActive(false);
            handle.gameObject.SetActive(false);
        }
    }
}