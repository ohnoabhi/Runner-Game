using UnityEngine;

public class TransformSmoothMovement : PlayerMovement
{
    private Transform transform;

    public TransformSmoothMovement(PlayerController playerController, float speed, float slideSpeed,
        float slideSmoothness, float xClamp) :
        base(playerController, speed, slideSpeed, slideSmoothness, xClamp)
    {
        transform = playerController.transform;
    }

    public override void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
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
            deltaX = Input.GetAxis("Mouse X") / Screen.width * 100;
        }
        else
        {
            deltaX = 0;
        }

        var position = transform.position;
        var movement = position;
        movement.z += speed * Time.deltaTime;

        position = movement;
        transform.position = position;
        movement.x += deltaX * slideSpeed;
        // movement.x = Mathf.Clamp(movement.x, -XMovementClamp, XMovementClamp);
        position = Vector3.MoveTowards(position, movement, slideSmoothness * Time.deltaTime);
        // position = movement;
        transform.position = position;
        var quaternion = Quaternion.Euler(0, deltaX == 0
            ? 0
            : (deltaX < 0
                ? -10
                : 10), 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            quaternion, 60 * Time.deltaTime);
    }
}