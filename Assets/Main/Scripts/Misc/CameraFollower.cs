using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    private Vector3 offset;

    public static Action<GameObject> FollowerChange;
    private void Start()
    {
        offset = transform.position - player.transform.position;

    }

    private void OnEnable()
    {
        FollowerChange += changeFollower;    
    }

    private void OnDisable()
    {
        FollowerChange -= changeFollower;
    }

    public void changeFollower(GameObject newPlayer)
    {
        player = newPlayer;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
