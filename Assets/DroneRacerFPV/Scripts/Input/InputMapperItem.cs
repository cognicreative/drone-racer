using System;

namespace DroneRacerFpv.Input
{
    [Serializable]
    public class InputMapperItem
    {
        public string axisKeyName;
        public string axisName;
        public float value;
        public bool valueChanged;
        public float minValue;
        public float maxValue;
        public float trim;
        public float scale;
        public float deadzone;
        public float invertValue;

        public InputMapperItem()
        {
        }

        public InputMapperItem(InputMapperItem m)
        {
            axisKeyName = m.axisKeyName;
            axisName = m.axisName;
            value = m.value;
            valueChanged = m.valueChanged;
            minValue = m.minValue;
            maxValue = m.maxValue;
            trim = m.trim;
            deadzone = m.deadzone;
            scale = m.scale;
            invertValue = m.invertValue;
        }

        public void InitValues()
        {
            value = 0;
            valueChanged = false;
            minValue = 0;
            maxValue = 0;
            deadzone = 0.025f;
        }

        public float ApplyInvertTrimScale(float v)
        {
            v *= invertValue;

            v += trim;

            v *= scale;

            return v;
        }

        public bool UpdateValue(float v)
        {
            bool retVal = false;

            if (v != value)
            {
                valueChanged = true;

                if (v < minValue)
                {
                    minValue = v;
                }

                if (v > maxValue)
                {
                    maxValue = v;
                }

                value = v;

                retVal = true;
            }

            return retVal;
        }

        public float GetAxisRange()
        {
            return maxValue - minValue;
        }
    }
}