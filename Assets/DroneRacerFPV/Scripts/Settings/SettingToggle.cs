using DroneRacerFpv.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace DroneRacerFpv.Settings
{
    [RequireComponent(typeof(Toggle))]
    public class SettingToggle : MonoBehaviour
    {
        public string settingName;

        Toggle toggle;

        void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnValueChanged);

            SetValue(GetValue());
        }

        void OnValueChanged(bool value)
        {
            SetValue(value, false);
        }

        public void SetValue(bool value, bool updateToggle = true)
        {
            PlayerPrefs.SetInt(settingName, value ? 1 : 0);

            DroneRacer.UpdateSettings();

            if (updateToggle == true)
            {
                toggle.isOn = value;
            }
        }

        public bool GetValue()
        {
            return PlayerPrefs.GetInt(settingName) == 1;
        }
    }
}