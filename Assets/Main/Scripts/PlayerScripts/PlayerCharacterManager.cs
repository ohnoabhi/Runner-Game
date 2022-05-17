using CharacterBase;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
    private void Start()
    {
        Player.Instance.OnChangeHealthLevel += ChangeSkin;
        ChangeSkin(0);
    }

    private void OnDestroy()
    {
        Player.Instance.OnChangeHealthLevel -= ChangeSkin;
    }

    private void ChangeSkin(int count)
    {
        var skinPrefab = CharacterDatabase.Get().CurrentCharacterList.GetSkin(count);
        if (!skinPrefab) return;

        var characterInstance = Instantiate(skinPrefab, transform);
        characterInstance.transform.localPosition = Vector3.zero;
        characterInstance.transform.localRotation
            = Quaternion.identity;

        if (Player.Instance.Character)
            Destroy(Player.Instance.Character.gameObject);

        Player.Instance.Character = characterInstance;

        //making sure its on floor
        characterInstance.transform.position =
            new Vector3(characterInstance.transform.position.x, 0, characterInstance.transform.position.z);
    }
}