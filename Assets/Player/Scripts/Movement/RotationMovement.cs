using UnityEngine;

public class RotationMovement : PlayerMovement
{
    private Transform movementTransform;
    private Transform rotationTransform;
    private float screeX;


    public override void MoveForward()
    {
        movementTransform.Translate(rotationTransform.forward * speed * Time.deltaTime, Space.World);
        if (Mathf.Abs(movementTransform.position.x) > 2.5f)
        {
            var position = movementTransform.position;
            position = new Vector3(Mathf.Clamp(position.x, -2.5f, 2.5f), position.y, position.z);
            movementTransform.position = position;
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
            // if (Mathf.Abs(deltaX) < 0.1f) deltaX = 0;
        }

        var quaternion = Quaternion.Euler(0, deltaX == 0
            ? 0
            : (deltaX < 0
                ? -10
                : 10), 0);
        rotationTransform.rotation = Quaternion.RotateTowards(rotationTransform.rotation,
            quaternion, slideSpeed * 10 * Time.deltaTime);

        movementTransform.Translate(Vector3.right * deltaX * speed * Time.deltaTime, Space.World);
        if (!(Mathf.Abs(movementTransform.position.x) > 2.5f)) return;
        var position = movementTransform.position;
        position = new Vector3(Mathf.Clamp(position.x, -2.5f, 2.5f), position.y, position.z);
        movementTransform.position = position;
    }

    public RotationMovement(PlayerController playerController, float speed, float slideSpeed, float slideSmoothness,
        float xClamp, Transform rotationTransform) : base(playerController, speed, slideSpeed, slideSmoothness, xClamp)
    {
        movementTransform = playerController.transform;
        this.rotationTransform = rotationTransform;
        screeX = 1f / Screen.width;
    }
}