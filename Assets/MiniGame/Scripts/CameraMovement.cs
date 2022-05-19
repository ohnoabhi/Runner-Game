using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float slideSpeed = 0.05f;

    [SerializeField]
    float XMovementClamp = 0.01f;

    [SerializeField]
    float XMovementClampNegative = 0.01f;

    [SerializeField]
    float speed = 5f;
    void Update()
    {

        var movement = transform.position;
        var slide = Vector3.zero;
#if UNITY_EDITOR
        slide += Slide(Input.GetAxisRaw("Horizontal") * slideSpeed);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (transform.position.x < 4 && transform.position.x > - 4)
                {
                   slide += Slide(-(touch.deltaPosition.x) * playerStats.turningSpeed);
                }
            }
        }
#endif
       // movement += Vector3.forward * speed * Time.deltaTime;
        movement += slide;
        movement.x = Mathf.Clamp(movement.x, -XMovementClampNegative, XMovementClamp);

        transform.position = movement;
    }
    private Vector3 Slide(float amount)
    {
        var position = new Vector3(
            amount,
            0,
            0
        );
        return position;
    }
}
