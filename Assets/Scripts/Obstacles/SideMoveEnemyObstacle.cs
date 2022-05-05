using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMoveEnemyObstacle : MonoBehaviour,IObstacle
{
    int limit = 4;

    [SerializeField]
    int damage;

    bool side = true;

    public void Collide()
    {
        PlayerLevelManager.instance.Remove(damage);
        InGameUI.RefreshStats?.Invoke();
    }

    private void FixedUpdate()
    {
        if (side)
        {
            if (transform.position.x <= 4)
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
            if (transform.position.x >= -4)
            {
                StartCoroutine(EnemyMover(-0.1f));
            }
            else
            {
                side = !side;
            }

        }
    }

    IEnumerator EnemyMover(float offset)
    {
        yield return new WaitForSeconds(0.1f);

        transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }
}
