using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMoveEnemyObstacle : MonoBehaviour, IObstacle
{
    private int limit = 4;

    [SerializeField] private int damage;

    private bool side = true;

    public void Collide()
    {
        Player.Instance.TakeDamage(damage);
    }

    private void FixedUpdate()
    {
        if (side)
        {
            if (transform.position.x <= limit)
            {
                StartCoroutine(EnemyMover(0.1f));
            }
            else
            {
                side = !side;
            }
        }

        else
        {
            if (transform.position.x >= -limit)
            {
                StartCoroutine(EnemyMover(-0.1f));
            }
            else
            {
                side = !side;
            }
        }
    }

    private IEnumerator EnemyMover(float offset)
    {
        yield return new WaitForSeconds(0.1f);

        transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }
}