using UnityEngine;

public abstract class PlayerMovement
{
    protected PlayerController PlayerController;
    protected float speed;
    protected float slideSpeed;
    protected float slideSmoothness;
    protected float xClamp;

    protected PlayerMovement(PlayerController playerController, float speed, float slideSpeed, float slideSmoothness,
        float xClamp)
    {
        PlayerController = playerController;
        this.speed = speed;
        this.slideSpeed = slideSpeed;
        this.slideSmoothness = slideSmoothness;
        this.xClamp = xClamp;
    }

    public abstract void MoveForward();
    public abstract void MoveSideways();
}