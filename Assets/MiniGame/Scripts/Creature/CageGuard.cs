using System.Threading.Tasks;
using UnityEngine;

public class CageGuard : MonoBehaviour
{
    [SerializeField] float speed;

    public async void Run(Vector3 target)
    {
        GetComponent<Animator>().SetBool("Run", true);
        while (transform != null && transform.position != target)
        {
            var targetPosition =
                Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.position = targetPosition;

            await Task.Yield();
        }

        //Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.other.name == "FinalSoliderPos")
        {
            Destroy(gameObject);
        }
    }
}