using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 Axis;

    [SerializeField] private float Speed;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(Speed * Time.deltaTime * Axis);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}