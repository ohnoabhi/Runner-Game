using UnityEngine;

public class Step : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private Renderer[] renderers;

    public ParticleSystem[] GetParticles()
    {
        return particles;
    }

    public void SetColor()
    {
        var property = new MaterialPropertyBlock();
        property.SetColor("_Color", Random.ColorHSV(0.1f, 0.8f, 1f, 1f, 0.5f, 1f));
        foreach (var renderer in renderers)
        {
            renderer.SetPropertyBlock(property);
        }
    }
}