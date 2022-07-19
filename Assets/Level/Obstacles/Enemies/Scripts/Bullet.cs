using System.Threading.Tasks;
using Stats;
using UnityEngine;

namespace Soldiers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 300;

        public async void Trigger(PlayerController player, int damage)
        {
            var target = player.transform.position;
            target.y = transform.position.y;
            target.z += Random.Range(1, 2f);
            while (transform.position != target)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed);
                var direction = target - transform.position;
                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(direction);
                await Task.Yield();
            }

            // if (Random.value > 0.3f)
            player.TakeDamage(damage +
                              (damage * Mathf.FloorToInt((StatsManager.Get(StatType.PlayerStat) - 1) * 0.25f)));
            Destroy(gameObject);
        }
    }
}