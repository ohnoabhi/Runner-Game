using System.Threading.Tasks;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Player player;
    public float Damage = 0.25f;


    public async Task StartAttack(Vector3 position, Player player)
    {
        if (!animator.GetBool("Running"))
            animator.SetBool("Running", true);
        while (transform.position != position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 5 * Time.deltaTime);
            await Task.Yield();
        }

        this.player = player;
    }

    private void Update()
    {
        if (!player) return;

        if (!animator.GetBool("IsAttacking"))
            animator.SetBool("IsAttacking", true);
    }
}