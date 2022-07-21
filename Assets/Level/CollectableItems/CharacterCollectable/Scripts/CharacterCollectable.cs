using System;
using UnityEngine;

public class CharacterCollectable : CollectableItem
{
    [SerializeField] private Transform visual;
    private Character character;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        foreach (Transform child in visual)
        {
            Destroy(child.gameObject);
        }

        character = Instantiate(Shop.GetCharacter(), visual);
        character.Animator.SetTrigger("Idle");
        character.UI.SetPlayerHealth(amount);


        var requiredForIncrement = amount / (GameManager.Instance.HealthRequiredForIncrement - 1);
        var startSize = Vector3.one * GameManager.Instance.PlayerStartSize;
        var requiredSize =
            startSize + (startSize * (requiredForIncrement * GameManager.Instance.HealthIncrementPercentage));

        if (requiredSize.x > GameManager.Instance.PlayerMaxSize)
            requiredSize = Vector3.one * GameManager.Instance.PlayerMaxSize;

        visual.localScale = requiredSize;
        character.UITransform.localScale = Vector3.one * (1 / requiredSize.x);
        character.UITransform.localScale *= 1 / character.transform.localScale.x;
        character.transform.rotation = Quaternion.Euler(0, 0, 0);
        character.UITransform.rotation = Quaternion.Euler(0, 0, 0);
        Show();
    }
}