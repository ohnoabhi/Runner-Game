using System;
using UnityEngine;

public class SpinnerObstacle : Obstacle
{
    [SerializeField] private float speed = 10;
    [SerializeField] private Vector3 direction;
    private bool rotate;

    protected override void OnEnable()
    {
        base.OnEnable();
        rotate = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        rotate = false;
    }

    private void Update()
    {
        if (rotate) transform.rotation *= Quaternion.Euler(speed * Time.deltaTime * direction);
    }

    protected override void OnCollide(PlayerController playerController, Vector3 collisionPoint)
    {
        rotate = false;
    }
}