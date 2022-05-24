using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Settings;

public class SettingsController : MonoBehaviour
{
    Animator settingsAnim;

    [SerializeField]
    SettingsButton[] settingsBtns;

    bool sound = true;
    bool heptic = true;
    bool settingsBool = true;

    private void Awake()
    {
        settingsAnim = GetComponent<Animator>();
        
        foreach(var button in settingsBtns)
        {
            button.onClick.AddListener(() => { OnClickButton(button); });
            ToggleButton(button);
        }
    }

    private void ToggleButton(SettingsButton button)
    {
        switch(button.id)
        {
            case "Sound":
                button.Enabled = sound;
                break;

            case "Heptic":
                button.enabled = heptic;
                break;
        }
    }

    private void OnClickButton(SettingsButton button)
    {
        switch(button.id)
        {
            case "Sound":
                sound = !sound;
                ToggleButton(button);
                break;

            case "Heptic":
                heptic = !heptic;
                ToggleButton(button);
                break;
        }
    }

    public void Togglepanel()
    {
        if(settingsBool)
        {
            settingsAnim.SetBool("On", true);
            settingsAnim.SetBool("Off", false);
            settingsBool = !settingsBool;
        }
        else
        {
            settingsAnim.SetBool("On", false);
            settingsAnim.SetBool("Off", true);
            settingsBool = !settingsBool;
        }
            
    }

   /* private void OnEnable()
    {
        settingsAnim.SetBool("On", true);
        settingsAnim.SetBool("Off", false);
        
    }

    private void OnDisable()
    {
        settingsAnim.SetBool("On", true);
        settingsAnim.SetBool("Off", false);
    }*/
}
