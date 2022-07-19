using Soldiers;
using UnityEngine;

public class Tank : Obstacle
{
    [SerializeField] private float attackRadius = 10;
    [SerializeField] private float findPlayerIntervel = 1;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    private float findPlayerTime;


    [Range(0.1f, 1)] [SerializeField] private float fireRate;
    [SerializeField] private Bullet shell;
    [SerializeField] private Transform muzzle;
    private PlayerController _playerController;
    [SerializeField] private ParticleSystem deathEffect;

    private bool isDead = false;

    private void Update()
    {
        if (isDead) return;

        if (_playerController)
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

    protected override void OnCollide(PlayerController playerController, Vector3 collisionPoint)
    {
        AudioManager.Play("TankDie");
        Die();
    }

    public void Die()
    {
        if (isDead) return;
        deathEffect.Play();
        _playerController = null;
        isDead = true;
        animator.SetTrigger("Die");
    }

    public void ResetItem()
    {
        _playerController = null;
        isDead = false;
        animator.SetTrigger("Reset");
    }

    private void Attack()
    {
        if (_playerController == null ||
            Vector3.Distance(_playerController.transform.position, transform.position) > attackRadius)
        {
            _playerController = null;
            return;
        }

        transform.rotation = Quaternion.LookRotation(_playerController.transform.position - transform.position);
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
            instance.Trigger(_playerController, damage);
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
            if (!item.CompareTag("Player") || !item.GetComponent<PlayerController>()) continue;
            _playerController = item.GetComponent<PlayerController>();
            break;
        }
    }

    public static bool Get(Collision collision, out Tank tank)
    {
        tank = collision.transform.GetComponent<Tank>();
        return tank;
    }
}