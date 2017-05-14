using DroneRacerFpv.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace DroneRacerFpv.Settings
{
    [RequireComponent(typeof(ToggleGroup))]
    public class SettingToggleGroup : MonoBehaviour
    {
        public string settingName;

        public Toggle[] toggleAry;

        void Awake()
        {
            foreach (Toggle t in toggleAry)
            {
                t.onValueChanged.AddListener(OnValueChanged);
            }

            SetValue(GetValue());
        }

        void OnValueChanged(bool value)
        {
            if (value == true)
            {
                for (int i = 0; i < toggleAry.Length; i++)
                {
                    if (toggleAry[i].isOn == true)
                    {
                        SetValue(i, false);
                    }
                }
            }
        }

        public void SetValue(int idx, bool updateToggle = true)
        {
            PlayerPrefs.SetInt(settingName, idx);

            DroneRacer.UpdateSettings();

            if (updateToggle == true)
            {
                toggleAry[idx].isOn = true;
            }
        }

        public int GetValue()
        {
            return PlayerPrefs.GetInt(settingName);
        }
    }
}