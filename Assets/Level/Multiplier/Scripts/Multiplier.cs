using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Multiplier : MonoBehaviour, IObstacle
{
    [Serializable]
    public class MultiplierValue
    {
        public int LeftValue;
        public int RightValue;
    }

    public MultiplierValue Value;
    [SerializeField] private ParticleSystem[] leftParticles;
    [SerializeField] private ParticleSystem[] rightParticles;
    [SerializeField] private TextMeshPro[] texts;
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;

    public void Init(MultiplierValue value)
    {
        Value = value;

        foreach (var leftParticle in leftParticles)
        {
            var color = Value.LeftValue > 0 ? positiveColor : negativeColor;
            var main = leftParticle.main;
            color.a = 1;
            main.startColor = color;
        }

        foreach (var rightParticle in rightParticles)
        {
            var color = Value.RightValue > 0 ? positiveColor : negativeColor;
            var main = rightParticle.main;
            color.a = 1;
            main.startColor = color;
        }

        texts[0].text = (Value.LeftValue > 0 ? "+" : "") + Value.LeftValue + " Ft";
        texts[1].text = (Value.RightValue > 0 ? "+" : "") + Value.RightValue + "Ft";
    }

    private bool collided = false;

    public void Collide(PlayerController playerController, Vector3 collisionPoint)
    {
        if (collided) return;
        collided = true;
        var x = playerController.transform.position.x;
        var value = x < transform.position.x ? Value.LeftValue : Value.RightValue;

        if (value > 0)
        {
            playerController.GainHealth(value);
        }
        else
        {
            playerController.TakeDamage(Mathf.Abs(value));
        }

        Destroy(gameObject);
    }
}