using System;
using CharacterBase;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
    private Character character;
    public Character Character => character;

    private void Start()
    {
        GameManager.OnPlayerHealthLevelChange += ChangeSkin;
        ChangeSkin(0);
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerHealthLevelChange -= ChangeSkin;
    }

    private void ChangeSkin(int count)
    {
        var skinPrefab = CharacterDatabase.Get().CurrentCharacterList.GetSkin(count);
        if (!skinPrefab) return;

        var characterInstance = Instantiate(skinPrefab, transform);
        characterInstance.transform.localPosition = Vector3.zero;
        characterInstance.transform.localRotation
            = Quaternion.identity;

        if (character)
            Destroy(character.gameObject);

        character = characterInstance;

        //making sure its on floor
        characterInstance.transform.position =
            new Vector3(characterInstance.transform.position.x, 0, characterInstance.transform.position.z);
    }
}