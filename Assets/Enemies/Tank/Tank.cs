using System.Collections;
using System.Collections.Generic;
using Soldiers;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private float attackRadius = 10;
    [SerializeField] private float findPlayerIntervel = 1;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    private float findPlayerTime = 0;


    [Range(0.1f, 1)] [SerializeField] private float fireRate;
    [SerializeField] private Bullet shell;
    [SerializeField] private Transform muzzle;
    private Player player;

    private bool isDead = false;

    private void Update()
    {
        if (isDead) return;

        if (player)
        {
            Attack();
            findPlayerTime = 0;
            return;
        }

        if (findPlayerTime >= findPlayerIntervel)
        {
            LookForPlayer();
            findPlayerTime = 0;
        }
        else
        {
            findPlayerTime += Time.deltaTime;
        }
    }

    public void Die()
    {
        if (isDead) return;
        player = null;
        isDead = true;
        animator.SetTrigger("Die");
    }

    private void Attack()
    {
        if (player == null || Vector3.Distance(player.transform.position, transform.position) > attackRadius)
        {
            player = null;
            return;
        }

        if (Shoot())
            animator.SetTrigger("Shoot");
    }

    private float shootTime = 0;
    private float timeToShoot = 0;

    private void Start()
    {
        timeToShoot = 0.1f / fireRate;
    }

    public bool Shoot()
    {
        if (shootTime >= timeToShoot)
        {
            var instance = Instantiate(shell, muzzle.position, Quaternion.identity);
            instance.transform.forward = muzzle.forward;
            instance.Trigger(player.transform.position);
            shootTime = 0;
            return true;
        }
        else
        {
            shootTime += Time.deltaTime;
            return false;
        }
    }

    private void LookForPlayer()
    {
        var objects = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);

        foreach (var item in objects)
        {
            if (!item.CompareTag("Player") || !item.GetComponent<Player>()) continue;
            player = item.GetComponent<Player>();
            break;
        }
    }

    public static bool Get(Collision collision, out Tank tank)
    {
        tank = collision.transform.GetComponent<Tank>();
        return tank;
    }
}