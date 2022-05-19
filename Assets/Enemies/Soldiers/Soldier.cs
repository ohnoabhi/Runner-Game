using UnityEngine;

namespace Soldiers
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField] private float attackRadius = 10;
        [SerializeField] private float findPlayerIntervel = 1;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Gun gun;
        [SerializeField] private Animator animator;
        private float findPlayerTime = 0;

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

            if (gun.Shoot(player.transform))
                animator.SetTrigger("Shoot");
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

        public static bool Get(Collision collision, out Soldier soldier)
        {
            soldier = collision.transform.GetComponent<Soldier>();
            return soldier;
        }
    }
}