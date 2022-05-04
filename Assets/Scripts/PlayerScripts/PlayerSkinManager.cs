using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField]
    SkinItem[] skins;

    [SerializeField]
    GameObject playerSkin;

    public static Action<int> MeshChange;

    private void OnEnable()
    {
        MeshChange += ChangeSkin;
    }

    private void OnDisable()
    {
        MeshChange -= ChangeSkin;
    }

    public void ChangeSkin(int count)
    {
        GameObject nextSkin = Instantiate(skins[count].skin, transform);
        nextSkin.transform.position = playerSkin.transform.position;
        nextSkin.transform.rotation = playerSkin.transform.rotation;
        Destroy(playerSkin.gameObject);

        playerSkin = nextSkin;
    }
}

[System.Serializable]
public class SkinItem
{
    public int num;
    public GameObject skin;
}

