using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SideMoveEnemyObstacle : MonoBehaviour, IObstacle
{
    private int limit = 4;

    [SerializeField] private Animator animator;

    [SerializeField] private int damage;
    [SerializeField] private float speed = 50;

    private bool isMoving;

    public void Collide(Player player)
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    private void Start()
    {
        animator.SetTrigger("Walk");
        isMoving = true;
        Move(limit);
    }

    private async void Move(float x)
    {
        var target = new Vector3(x, transform.position.y, transform.position.z);

        while (transform.position != target)
        {
            if (!transform) isMoving = false;
            if (!isMoving) break;

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(target - transform.position);
            await Task.Yield();
        }

        if (isMoving)
            Move(x * -1);
    }
}