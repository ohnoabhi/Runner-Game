using System.Threading.Tasks;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public async Task StartAttack(Vector3 position)
    {
        animator.SetTrigger("Run");
        while (transform.position != position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 5 * Time.deltaTime);
            await Task.Yield();
        }

        animator.SetTrigger("Attack");
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
    }
}