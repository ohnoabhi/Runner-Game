using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform Target;

    private Vector3 offset;

    private void Start()
    {
        if (Target)
            offset = transform.position - Target.transform.position;
    }

    private void LateUpdate()
    {
        if (Target)
            transform.position = Target.transform.position + offset;
    }
}