using System.Threading.Tasks;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    [SerializeField] private float Limit = 4;
    [SerializeField] private float Speed = 5;
    [SerializeField] private float Delay = 5;

    private bool isMoving;
    private new Transform transform;

    private void Start()
    {
        isMoving = true;
        transform = gameObject.transform;
        Move(Limit);
    }

    private async void Move(float x)
    {
        while (true)
        {
            if (transform == null) return;

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