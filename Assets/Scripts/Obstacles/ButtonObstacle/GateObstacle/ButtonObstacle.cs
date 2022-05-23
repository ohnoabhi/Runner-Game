using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonObstacle : MonoBehaviour
{
    [SerializeField] private bool isMultiColored = false;

    [SerializeField] private TriggerButton[] buttons;

    [ShowIf("isMultiColored")]
    [SerializeField] private Renderer[] renderers;

    private void Start()
    {
        if (buttons.Length == 0) return;
        if (isMultiColored)
        {
            var active = Random.Range(0, buttons.Length);
            var i = 0;
            foreach (var button in buttons)
            {
                button.TriggerEvent.RemoveAllListeners();
                if (i == active)
                {
                    button.TriggerEvent.AddListener(Open);
                    ApplyColor(button.GetColor());
                }

                i++;
            }
        }
        else
        {
            var active = 1;
            var i = 0;
            foreach (var button in buttons)
            {
                button.TriggerEvent.RemoveAllListeners();
                button.gameObject.SetActive(false);
                if (i == active)
                {
                    button.TriggerEvent.AddListener(Open);
                    button.gameObject.SetActive(true);
                }

                i++;
            }
        }
    }

    private void ApplyColor(Color color)
    {
        if (renderers.Length <= 0) return;

        var property = new MaterialPropertyBlock();
        property.SetColor("_Color", color);
        foreach (var renderer in renderers)
        {
            renderer.SetPropertyBlock(property);
        }
    }


    public void Open()
    {
        GetComponent<Animator>().SetBool("Play", true);
    }
}