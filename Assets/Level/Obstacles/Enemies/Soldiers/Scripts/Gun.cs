using System;
using UnityEngine;

namespace Soldiers
{
    public class Gun : MonoBehaviour
    {
        [Range(0.1f, 1)] [SerializeField] private float fireRate;
        [SerializeField] private Bullet shell;
        [SerializeField] private Transform muzzle;

        private float shootTime = 0;
        private float timeToShoot = 0;

        public void SetFireRate(float fireRate)
        {
            this.fireRate = fireRate;
        }

        private void Start()
        {
            timeToShoot = 0.1f / fireRate;
        }

        public bool Shoot(PlayerController player)
        {
            if (shootTime >= timeToShoot)
            {
                var instance = Instantiate(shell, muzzle.position, Quaternion.identity);
                instance.transform.forward = muzzle.forward;
                instance.Trigger(player);
                shootTime = 0;
                return true;
            }
            else
            {
                shootTime += Time.deltaTime;
                return false;
            }
        }
    }
}