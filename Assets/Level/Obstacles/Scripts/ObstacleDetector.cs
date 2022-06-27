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
        var obstacle = other.GetComponent<IObstacle>();

        if (obstacle != null)
        {
            hitEffect.transform.position = other.ClosestPoint(_playerController.transform.position);
            hitEffect.Play();
            obstacle.Collide(_playerController);
        }
        else if (other.CompareTag("FinishLine"))
        {
            GameManager.Instance.OnFinish(true);
        }
        else if (other.CompareTag("Pitfall"))
        {
            _playerController.Fall();
        }
    }
}