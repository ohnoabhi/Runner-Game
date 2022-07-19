using UnityEngine;

public class RotationMovement : PlayerMovement
{
    private Transform movementTransform;
    private Transform rotationTransform;
    private float screeX;

    public override void MoveForward()
    {
        if (onHit)
        {
            if (movementTransform.position.z > hitPosition.z - 2)
            {
                movementTransform.Translate(rotationTransform.forward * -(speed * 0.5f) * Time.deltaTime, Space.World);
            }

            if (movementTransform.position.z <= hitPosition.z - 2) onHit = false;
        }
        else
        {
            movementTransform.Translate(rotationTransform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    public override void MoveSideways()
    {
        float deltaX;
        if (Input.GetMouseButtonDown(0))
        {
            deltaX = 0;
        }
        else if (Input.GetMouseButton(0))
        {
            deltaX = Input.GetAxis("Mouse X");
        }
        else
        {
            deltaX = 0;
        }

        if (deltaX != 0)
        {
            deltaX = deltaX * 500 * screeX;
        }

        var quaternion = Quaternion.Euler(0, deltaX == 0
            ? 0
            : (deltaX < 0
                ? -10
                : 10), 0);
        rotationTransform.rotation = Quaternion.RotateTowards(rotationTransform.rotation,
            quaternion, slideSpeed * Time.deltaTime);

        movementTransform.Translate(Vector3.right * deltaX * slideMoveSpeed * speed * Time.deltaTime, Space.World);
        if (!(Mathf.Abs(movementTransform.position.x) > 2.5f)) return;
        var position = movementTransform.position;
        position = new Vector3(Mathf.Clamp(position.x, -2.5f, 2.5f), position.y, position.z);
        movementTransform.position = position;
    }

    public RotationMovement(PlayerController playerController, float speed, float slideSpeed, float slideMoveSpeed,
        float xClamp, Transform rotationTransform) : base(playerController, speed, slideSpeed, slideMoveSpeed, xClamp)
    {
        movementTransform = playerController.transform;
        this.rotationTransform = rotationTransform;
        screeX = 1f / Screen.width;
    }
}