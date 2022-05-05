using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] SkinItem[] skins;

    [SerializeField] GameObject playerSkin;

    private void OnEnable()
    {
        Player.Instance.OnChangeHealthLevel += ChangeSkin;
    }

    private void OnDisable()
    {
        Player.Instance.OnChangeHealthLevel += ChangeSkin;
    }

    private void ChangeSkin(int count)
    {
        var skinInstance = Instantiate(skins[count].Skin, transform);
        skinInstance.transform.position = playerSkin.transform.position;
        skinInstance.transform.rotation = playerSkin.transform.rotation;
        Destroy(playerSkin.gameObject);

        playerSkin = skinInstance;
    }
}

[System.Serializable]
public class SkinItem
{
    public GameObject Skin;
}