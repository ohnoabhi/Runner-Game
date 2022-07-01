using System.Threading.Tasks;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    [SerializeField] private float Limit = 4;
    [SerializeField] private float Speed = 5;
    [SerializeField] private float Delay = 5;

    private bool isMoving;
    private new Transform transform;

    protected override void OnEnable()
    {
        base.OnEnable();
        isMoving = true;
        transform = gameObject.transform;
        transform.localPosition = new Vector3(0, 0.15f, 0);
        Move(Limit);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isMoving = false;
    }

    private async void Move(float x)
    {
        while (true)
        {
            if (transform == null) return;
            if (!isMoving)
            {
                break;
            }

            var target = new Vector3(x, transform.position.y, transform.position.z);

            while (transform != null && transform.position != target)
            {
                if (!transform) isMoving = false;
                if (!isMoving) break;

                transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
                // transform.rotation = Quaternion.LookRotation(target - transform.position);
                await Task.Yield();
            }

            await Task.Delay((int) (Delay * 1000));
            if (isMoving)
            {
                x = x * -1;
                continue;
            }

            break;
        }
    }
}