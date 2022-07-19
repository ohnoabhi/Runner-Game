using UnityEngine;

public class TransformSmoothMovement : PlayerMovement
{
    private Transform transform;
    private float screeX;

    public TransformSmoothMovement(PlayerController playerController, float speed, float slideSpeed, float xClamp) :
        base(playerController, speed, slideSpeed, 0, xClamp)
    {
        transform = playerController.transform;

        screeX = 1f / (Screen.height / (float) Screen.width);
    }

    public override void MoveForward()
    {
        var deltaX = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") : 0 * screeX;

        var newPosition = transform.position;

        {
            newPosition.z += speed * Time.deltaTime;
        }

        newPosition.x += deltaX * slideSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        transform.position = newPosition;
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.Euler(0, deltaX > 0 ? 20 : deltaX < 0 ? -20 : 0, 0), 50 * Time.deltaTime);
    }

    public override void MoveSideways()
    {
    }
}