using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothness;
    private bool follow;

    private void Start()
    {
        follow = true;
    }

    private void LateUpdate()
    {
        if (!follow || !Target) return;
        var target = Target.transform.position;
        target.x = 0;
        target += offset;
        var movement = target;
        transform.position = movement;
    }

    public void MoveToPosition(Vector3 position, Quaternion rotation)
    {
        Target = null;
        transform.DOMove(position, 1.5f);
        transform.DORotate(rotation.eulerAngles, 1.5f);
    }

    public async void SetOffset(Vector3 position)
    {
        while (offset.y != position.y)
        {
            offset.y = Mathf.MoveTowards(offset.y, position.y, 1 * Time.deltaTime);
            await Task.Yield();
        }
    }

    public async void SetOffset(Vector3 position, Quaternion rotation, bool animate = true)
    {
        offset = position;
        if (animate)
        {
            follow = false;
            var t = 0f;
            while (transform != null && Target != null && transform.position != Target.position + offset)
            {
                if (transform == null || Target == null)
                {
                    follow = true;
                    return;
                }

                transform.position = Vector3.Lerp(transform.position, Target.position + offset, t);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t);
                t += smoothness * Time.deltaTime;
                await Task.Yield();
                if (transform == null || Target == null)
                {
                    follow = true;
                    return;
                }
            }

            follow = true;
        }
        else
        {
            transform.position = Target.position + offset;
            transform.rotation = rotation;
        }
    }
}