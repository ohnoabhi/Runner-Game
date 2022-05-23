using System.Threading.Tasks;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        if (Target)
            transform.position = Target.transform.position + offset;
    }

    public async void MoveToPosition(Vector3 position, Quaternion rotation)
    {
        Target = null;
        var t = 0f;
        while (transform.position != position)
        {
            transform.position = Vector3.Slerp(transform.position, position, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t);
            t += 0.5f * Time.deltaTime;
            await Task.Yield();
        }
    }

    public async void SetOffset(Vector3 position, Quaternion rotation, bool animate = true)
    {
        offset = position;
        var t = 0f;
        if (animate)
            while (transform.rotation != rotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t);
                t += 0.5f * Time.deltaTime;
                await Task.Yield();
            }
        else
            transform.rotation = rotation;
    }
}