using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Multiplier : MonoBehaviour, IObstacle
{
    public enum MultiplierValueType
    {
        Add,
        Remove
    }

    [Serializable]
    public class MultiplierValue
    {
        public MultiplierValueType Type;
        public int amount;
    }

    [Serializable]
    public class MultiplierValuePair
    {
        public MultiplierValue LeftValue;
        public MultiplierValue RightValue;
    }

    public MultiplierValuePair[] AvailableValuesPairs;
    [SerializeField] private ParticleSystem[] leftParticles;
    [SerializeField] private ParticleSystem[] rightParticles;
    [SerializeField] private TextMeshPro[] texts;
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;
    private MultiplierValuePair current;

    private void Awake()
    {
        current = AvailableValuesPairs[Random.Range(0, AvailableValuesPairs.Length)];

        foreach (var leftParticle in leftParticles)
        {
            var color = current.LeftValue.Type == MultiplierValueType.Add ? positiveColor : negativeColor;
            var main = leftParticle.main;
            color.a = 1;
            main.startColor = color;
        }

        foreach (var rightParticle in rightParticles)
        {
            var color = current.RightValue.Type == MultiplierValueType.Add ? positiveColor : negativeColor;
            var main = rightParticle.main;
            color.a = 1;
            main.startColor = color;
        }

        texts[0].text = (current.LeftValue.Type == MultiplierValueType.Add ? "+" : "-") + current.LeftValue.amount;
        texts[1].text = (current.RightValue.Type == MultiplierValueType.Add ? "+" : "-") + current.RightValue.amount;
    }

    public void Collide(Player player)
    {
        var x = player.transform.position.x;
        var value = x < transform.position.x ? current.LeftValue : current.RightValue;

        if (value.Type == MultiplierValueType.Add)
        {
            player.GetComponent<PlayerHealth>().GainHealth(value.amount);
        }
        else
        {
            player.GetComponent<PlayerHealth>().TakeDamage(value.amount);
        }

        Destroy(gameObject);
    }
}