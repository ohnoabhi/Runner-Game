using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierRunner : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 3;

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
}