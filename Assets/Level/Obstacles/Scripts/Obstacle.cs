using UnityEngine;

public class Obstacle : MonoBehaviour, IObstacle
{
    [SerializeField] public int damage;
    [SerializeField] private bool shouldVibrate = true;
    [SerializeField] private bool dieOnTouch;
    private bool collided;

    protected virtual void OnEnable()
    {
        collided = false;
    }

    private void OnDisable()
    {
        collided = false;
    }

    public void Collide(PlayerController playerController)
    {
        if (collided) return;

        collided = true;
        if (shouldVibrate)
            VibrationManager.Vibrate(200);
        if (dieOnTouch)
        {
            playerController.Die();
        }
        else
        {
            playerController.TakeDamage(damage);
            OnCollide(playerController);
        }
    }

    protected virtual void OnCollide(PlayerController playerController)
    {
    }
}