using UnityEngine;

public class TransformMovement : PlayerMovement
{
    private Transform transform;
    private float screeX;

    public TransformMovement(PlayerController playerController, float speed, float slideSpeed, float slideSmoothness,
        float xClamp) :
        base(playerController, speed, slideSpeed, slideSmoothness, xClamp)
    {
        transform = playerController.transform;
        screeX = 1f / Screen.width;
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
            deltaX = Input.GetAxis("Mouse X");
        }
        else
        {
            deltaX = 0;
        }

        if (deltaX != 0) deltaX = deltaX * 500 * screeX;

        transform.Translate(Vector3.right * deltaX * slideSpeed * Time.deltaTime, Space.World);
        if (Mathf.Abs(transform.position.x) > 2.5f)
        {
            var position = transform.position;
            position = new Vector3(Mathf.Clamp(position.x, -2.5f, 2.5f), position.y, position.z);
            transform.position = position;
        }
    }
}