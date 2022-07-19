using UnityEngine;

public abstract class PlayerMovement
{
    protected PlayerController PlayerController;
    protected float speed;
    protected float slideSpeed;
    protected float slideMoveSpeed;
    protected float xClamp;
    protected bool onHit;
    protected Vector3 hitPosition;

    protected PlayerMovement(PlayerController playerController, float speed, float slideSpeed, float slideMoveSpeed,
        float xClamp)
    {
        PlayerController = playerController;
        this.speed = speed;
        this.slideSpeed = slideSpeed;
        this.slideMoveSpeed = slideMoveSpeed;
        this.xClamp = xClamp;
    }

    public abstract void MoveForward();
    public abstract void MoveSideways();

    public void Hit(Vector3 position)
    {
        onHit = true;
        hitPosition = position;
    }
}