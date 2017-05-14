using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DroneRacerFpv.Utils;

namespace DroneRacerFpv.Input
{
    public class InputMapper : MonoBehaviour
    {
        const int MAX_AXIS = 20;

        public InputMapperItem[] inputMapperItems = new InputMapperItem[MAX_AXIS];

        // Use this for initialization
        void Awake()
        {
            for (int i = 0; i < MAX_AXIS; i++)
            {
                InputMapperItem m = new InputMapperItem();

                m.axisName = string.Format("Axis {0}", i + 1);

                inputMapperItems[i] = m;
            }
        }

        public void Init()
        {
            for (int i = 0; i < MAX_AXIS; i++)
            {
                inputMapperItems[i].InitValues();
                inputMapperItems[i].value = InputHelper.GetInputAxis(inputMapperItems[i].axisName);
            }
        }

        float maxRange;
        public InputMapperItem SelectActiveAxis()
        {
            maxRange = 0;

            float range;

            InputMapperItem m;
            InputMapperItem retVal = null;

            for (int i = 0; i < MAX_AXIS; i++)
            {
                m = inputMapperItems[i];

                if (m.valueChanged == true)
                {
                    range = m.GetAxisRange();

                    if (maxRange < range)
                    {
                        maxRange = range;
                        retVal = inputMapperItems[i];
                    }
                }
            }

            return retVal;
        }

        void Update()
        {
            for (int i = 0; i < MAX_AXIS; i++)
            {
                inputMapperItems[i].UpdateValue(InputHelper.GetInputAxis(inputMapperItems[i].axisName));
            }
        }
    }
}