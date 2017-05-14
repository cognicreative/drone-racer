using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DroneRacerFpv.Utils;

namespace DroneRacerFpv.Ui
{
    [RequireComponent(typeof(Scrollbar))]
    public class ControlSlider : MonoBehaviour
    {
        public float min = -0.5f;
        public float max = 0.5f;

        float curValue;

        Scrollbar scrollBar;

        void Awake()
        {
            scrollBar = GetComponent<Scrollbar>();
            scrollBar.onValueChanged.AddListener(OnValueChanged);
            curValue = MathHelper.ScaleToRange(scrollBar.value, 0, 1, min, max);
        }

        void OnValueChanged(float value)
        {
            //Debug.Log("ControlSlider.OnValueChanged value=" + value);
            curValue = MathHelper.ScaleToRange(value, 0, 1, min, max);
        }

        public void SetValue(float value)
        {
            scrollBar.value = MathHelper.ScaleToRange(value, min, max, 0, 1);
            curValue = value;
        }

        public float GetValue()
        {
            return curValue;
        }
    }
}