using UnityEngine;

public class RotationSmoother : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.forward = Vector3.MoveTowards(transform.forward, target.forward, speed * Time.deltaTime);
        // transform.forward = Vector3.forward;
    }
}