using System.Collections;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    [SerializeField] private float Limit = 4;
    [SerializeField] private float Speed = 1;
    private new Transform transform;
    private bool shouldMove;
    private bool moveRight;
    private Coroutine moveCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        ResetMove();
    }

    public void ResetMove()
    {
        StopMove();
        transform = gameObject.transform;
        shouldMove = true;
        moveRight = true;
        var position = transform.localPosition;
        position.x = -Limit;
        transform.localPosition = position;

        moveCoroutine = StartCoroutine(Move());
    }

    
    public void StopMove()
    {
        shouldMove = false;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }


    private IEnumerator Move()
    {
        var position = transform.localPosition;
        if (moveRight)
        {
            position.x += Speed * Time.deltaTime;
        }
        else
        {
            position.x -= Speed * Time.deltaTime;
        }

        transform.localPosition = position;

        if (position.x >= Limit)
        {
            moveRight = false;
        }
        else if (position.x <= -Limit)
        {
            moveRight = true;
        }

        yield return null;
        if (shouldMove)
        {
            moveCoroutine = StartCoroutine(Move());
        }
    }

    protected override void OnDisable()
    {
        StopMove();
        base.OnDisable();
    }

    protected override void OnCollide(PlayerController playerController, Vector3 collisionPoint)
    {
        shouldMove = false;
    }
}