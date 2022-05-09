using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] SkinItem[] skins;

    [SerializeField] GameObject playerSkin;

    int currentSet = 0;

    private void Awake()
    {
        ChangeSkin(0);
    }

    private void Start()
    {
        Player.Instance.OnChangeHealthLevel += ChangeSkin;
        Player.Instance.OnChangeSet += ChangeSet;
    }

    private void OnDestroy()
    {
        Player.Instance.OnChangeHealthLevel += ChangeSkin;
        Player.Instance.OnChangeSet -= ChangeSet;
    }

    public void ChangeSet(int setNo)
    {
        currentSet = setNo;
        ChangeSkin(0);
    }

    private void ChangeSkin(int count)
    {
        var skinInstance = Instantiate(skins[currentSet].Skin[count], transform);
        skinInstance.transform.position = playerSkin.transform.position;
        skinInstance.transform.rotation = playerSkin.transform.rotation;
        Destroy(playerSkin.gameObject);

        playerSkin = skinInstance;

        //making sure its on floor
        skinInstance.transform.position = new Vector3(skinInstance.transform.position.x, 0, skinInstance.transform.position.z);

        CameraFollower.FollowerChange?.Invoke(skinInstance);

    }
}

[System.Serializable]
public class SkinItem
{
    public GameObject[] Skin;
}