using System.Threading.Tasks;
using UnityEngine;

public class BrakablePiece : MonoBehaviour
{
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Sleep();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Boss"))
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerController>();
                if (player && player.Health <= 20)
                {
                    return;
                }
            }

            WakeUp();
        }
    }

    public void Sleep()
    {
        if (rigidbody)
            rigidbody.isKinematic = true;
    }

    private async void WakeUp()
    {
        rigidbody.isKinematic = false;
        await Task.Delay(20);
        if (gameObject.activeInHierarchy)
            rigidbody.AddExplosionForce(30f, transform.position, 1);
    }
}