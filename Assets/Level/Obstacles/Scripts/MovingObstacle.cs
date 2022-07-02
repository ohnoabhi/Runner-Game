using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    [SerializeField] private float Limit = 4;
    [SerializeField] private float Duration = 2;
    [SerializeField] private float Delay = 1;

    private Sequence moveSequence;

    private bool isMoving;
    private new Transform transform;

    protected override void OnEnable()
    {
        base.OnEnable();
        isMoving = true;
        transform = gameObject.transform;
        transform.localPosition = new Vector3(-Limit, 0.15f, 0);
        moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOLocalMoveX(Limit, Duration).SetDelay(Delay));
        moveSequence.Append(transform.DOLocalMoveX(-Limit, Duration).SetDelay(Delay));
        moveSequence.SetLoops(-1, LoopType.Restart);
        moveSequence.Play();
        // Move(Limit);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        moveSequence?.Complete();
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

                transform.position = Vector3.MoveTowards(transform.position, target, Duration * Time.deltaTime);
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