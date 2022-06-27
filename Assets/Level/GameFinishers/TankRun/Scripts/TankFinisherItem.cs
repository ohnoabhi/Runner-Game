using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFinisherItem : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;


    public void PlayParticles()
    {
        AudioManager.Play("ConfettiBlast");
        foreach (var particle in particles)
        {
            particle.Play();
        }
    }
}