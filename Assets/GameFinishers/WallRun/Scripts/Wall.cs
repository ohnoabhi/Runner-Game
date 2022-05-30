using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshPro text;

    public void Init(bool locked, int multiplier)
    {
        lockIcon.SetActive(locked);
        text.gameObject.SetActive(!locked);
        text.text = "X" + multiplier;
    }
}