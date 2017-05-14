using UnityEngine;
using UnityEngine.UI;
using DroneRacerFpv.Utils;
using DroneRacerFpv.Controller;

namespace DroneRacerFpv.Settings
{
    [RequireComponent(typeof(Scrollbar))]
    public class SettingSlider : MonoBehaviour
    {
        public string settingName;
        public float min = -0.5f;
        public float max = 0.5f;

        public Text valueLabel;
        public string valueLabelFormat = "{0:0}";

        Scrollbar scrollBar;

        void Awake()
        {
            scrollBar = GetComponent<Scrollbar>();
            scrollBar.onValueChanged.AddListener(OnValueChanged);

            scrollBar.value = MathHelper.ScaleToRange(GetValue(), min, max, 0, 1);
            UpdateValueLabel();
        }

        void UpdateValueLabel()
        {
            if (valueLabel != null)
            {
                valueLabel.text = string.Format(valueLabelFormat, GetValue());
            }
        }

        void OnValueChanged(float value)
        {
            PlayerPrefs.SetFloat(settingName, MathHelper.ScaleToRange(value, 0, 1, min, max));
            UpdateValueLabel();
            DroneRacer.UpdateSettings();
        }

        public float GetValue()
        {
            float retVal = PlayerPrefs.GetFloat(settingName);

            //Debug.Log(string.Format("settingName={0}  value={1}  min={2}  max={3}", settingName, retVal, min, max));

            return retVal;
        }
    }
}