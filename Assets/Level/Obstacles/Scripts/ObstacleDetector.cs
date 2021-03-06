using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitEffect;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pitfall"))
        {
            _playerController.Fall();
        }

        var obstacle = other.GetComponent<IObstacle>();
        if (obstacle != null)
        {
            var closestPoint = other.ClosestPoint(_playerController.transform.position);
            hitEffect.transform.position = closestPoint;
            hitEffect.Play();
            obstacle.Collide(_playerController, closestPoint);
        }

        if (other.CompareTag("FinishLine"))
        {
            GameManager.Instance.OnFinish(true);
        }
    }
}