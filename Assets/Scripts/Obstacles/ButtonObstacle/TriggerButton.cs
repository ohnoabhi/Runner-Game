using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] private Color color;
    public UnityEvent TriggerEvent;

    [SerializeField] private new Renderer renderer;


    private void Start()
    {
        ApplyColor();
    }

    [Button("Apply Color")]
    private void ApplyColor()
    {
        if (!renderer) return;

        var property = new MaterialPropertyBlock();
        property.SetColor("_Color", color);
        renderer.SetPropertyBlock(property);
    }

    public void Trigger()
    {
        TriggerEvent?.Invoke();
    }

    public Color GetColor()
    {
        return color;
    }
}