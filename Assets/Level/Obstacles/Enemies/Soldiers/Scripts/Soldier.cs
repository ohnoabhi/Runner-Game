using UnityEngine;

namespace Soldiers
{
    public class Soldier : Obstacle
    {
        [SerializeField] private float attackRadius = 10;
        [SerializeField] private float findPlayerIntervel = 1;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] protected Gun gun;
        [SerializeField] protected Animator animator;
        private float findPlayerTime = 0;

        private PlayerController _playerController;

        private bool isDead = false;

        protected virtual void Update()
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

        protected override void OnCollide(PlayerController playerController)
        {
            AudioManager.Play("SoldierDie");
            Die();
        }

        public void Die()
        {
            if (isDead) return;
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
            if (gun.Shoot(_playerController))
            {
                AudioManager.Play("SoldierShoot");
                animator.SetTrigger("Shoot");
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

        public static bool Get(Collision collision, out Soldier soldier)
        {
            soldier = collision.transform.GetComponent<Soldier>();
            return soldier;
        }
    }
}