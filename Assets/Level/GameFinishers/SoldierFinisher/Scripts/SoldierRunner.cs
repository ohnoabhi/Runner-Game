using System.Threading.Tasks;
using Soldiers;
using UnityEngine;

public class SoldierRunner : Soldier
{
    [SerializeField] private float speed = 3;

    protected override void OnEnable()
    {
        base.OnEnable();
        gun.SetFireRate(Random.Range(0.4f, 0.5f));
    }

    public async Task Run(Vector3[] targets)
    {
        animator.SetTrigger("Walk");
        foreach (var target in targets)
        {
            while (transform.position != target)
            {
                var position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                transform.forward = position - transform.position;
                transform.position = position;
                await Task.Yield();
            }
        }

        Destroy(gameObject);
    }

    private bool canShoot = true;

    public void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    protected override void Update()
    {
        if (canShoot)
            base.Update();
    }
}