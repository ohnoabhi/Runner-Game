using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernamePopup : BasePopup
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button continueButton;

    protected override void OnShow()
    {
        nameInputField.text = Username.Name;
        continueButton.interactable = !string.IsNullOrEmpty(Username.Name);
        nameInputField.onValueChanged.AddListener(OnEndEdit);
    }

    private void OnEndEdit(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void OnClickContinue()
    {
        var name = nameInputField.text;
        if (string.IsNullOrEmpty(name))
        {
            continueButton.interactable = false;
            return;
        }

        Username.Name = name;
        Hide();
    }

    public void OnClickRandom()
    {
        nameInputField.text = RandomName.Name;
    }
}