using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonObstacle : MonoBehaviour
{
    [SerializeField] private string audio;
    [SerializeField] private bool isMultiColored = false;

    [SerializeField] private TriggerButton[] buttons;

    [ShowIf("isMultiColored")] [SerializeField]
    private Renderer[] renderers;

    public void Init()
    {
        Close();
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
            const int active = 1;
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

    private bool isOpen = false;

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        AudioManager.Play(audio);
        GetComponent<Animator>().SetBool("Play", true);
    }

    public void Close()
    {
        isOpen = false;
        GetComponent<Animator>().SetBool("Play", false);
    }
}