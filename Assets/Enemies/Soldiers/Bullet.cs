using System.Threading.Tasks;
using UnityEngine;

namespace Soldiers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10;

        public async void Trigger(Vector3 target)
        {
            target.y = transform.position.y;
            while (transform.position != target)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed);
                await Task.Yield();
            }

            Destroy(gameObject);
        }
    }
}