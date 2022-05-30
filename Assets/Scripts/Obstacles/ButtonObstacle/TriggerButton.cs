using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] private Color color;
    public UnityEvent TriggerEvent;

    [SerializeField] private new Renderer renderer;
    [SerializeField] private Transform button;

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
        button.DOLocalMoveY(-0.037f, 0.2f);
        TriggerEvent?.Invoke();
    }

    public Color GetColor()
    {
        return color;
    }
}