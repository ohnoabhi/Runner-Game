using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreatureItem : MonoBehaviour
{
    [SerializeField]
    int creatureId;

    [SerializeField]
    int keysRequired = 3;

    [SerializeField]
    Button unlockBtn;

    [SerializeField]
    TMP_Text buttonTxt;

    [SerializeField]
    GameObject cage;

    [SerializeField]
    GameObject creature;

    public int isUnlocked
    {
        get => PlayerPrefs.GetInt("CreatureItem" + creatureId.ToString(),0);

        set => PlayerPrefs.SetInt("CreatureItem" + creatureId.ToString(),value);
    }

    private void Start()
    {
        if(CreatureData.instance.ReturnCreature(creatureId) != null)
        {
            GameObject creatureReturned = CreatureData.instance.ReturnCreature(creatureId);

            Instantiate(creatureReturned, creature.transform.position, creature.transform.rotation, gameObject.transform);

            Destroy(creature.gameObject);

            creature = creatureReturned;

            //creature.transform.SetParent(this.transform);
        }

        if(isUnlocked == 0)
        {
            CageLocked();
        }

        else if(isUnlocked == 1)
        {
            CageUnlocked();
        }
    }

    private void CageUnlocked()
    {
        Debug.Log("Creature outside cage");

        buttonTxt.text = "Unlocked";

        cage.SetActive(false);
    }

    private void CageLocked()
    {
        Debug.Log("Creature in cage");

        unlockBtn.onClick.RemoveAllListeners();
        unlockBtn.onClick.AddListener(() => OnClickUnlock());

        buttonTxt.text = "Unlock";
    }

    public void OnClickUnlock()
    {
        int keys = KeyManager.instance.GetKeys();

        if(keys >= keysRequired)
        {
            isUnlocked = 1;
            KeyManager.instance.Remove(keysRequired);
            OpenCage();
        }

        else
        {
            Debug.Log("more keys required");
        }
    }

    private void OpenCage()
    {
        Debug.Log("cage open");

        buttonTxt.text = "Unlocked";

        cage.SetActive(false);

        GetComponentInParent<CreatureManager>().RefreshStats();
    }
}
