using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsButton : Button
    {
        public string id = "";
        [SerializeField] private Image image;
        [SerializeField] private Sprite enabledIcon;
        [SerializeField] private Sprite disabledIcon;

        private new bool enabled;

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                Enable();
            }
        }

        public void Enable()
        {
            image.sprite = Enabled ? enabledIcon : disabledIcon;
        }
    }
}

