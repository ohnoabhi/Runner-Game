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

    [SerializeField]
    Transform finalPos;

    [SerializeField]
    float speed;

    bool isRunning = false;

    public Transform soliderFinalPos;

    [SerializeField]
    CageGaurds[] gaurds;

    public Vector3 cameraOffset;



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

             var newCreature =Instantiate(creatureReturned, creature.transform.position, creature.transform.rotation, gameObject.transform);

            Destroy(creature.gameObject);

            creature = newCreature;

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

        creature.SetActive(false);

        foreach(var gaurd in gaurds)
        {
            gaurd.gameObject.SetActive(false);
        }
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

        isRunning = true;

        foreach (var gaurd in gaurds)
        {
            gaurd.isRunning = true;
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        if(creature.transform.position != finalPos.position)
        {
            Vector3 pos = Vector3.MoveTowards(creature.transform.position, finalPos.position,speed * Time.deltaTime);
            creature.GetComponent<Rigidbody>().MovePosition(pos);
            creature.GetComponent<Animator>().SetBool("Running", true);
        }

        else if(creature.transform.position == finalPos.position)
        {
            Destroy(creature);
            isRunning = false;
        }

        
    }
}
