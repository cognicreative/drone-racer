using System;
using System.Collections.Generic;
using UnityEngine;

namespace DroneRacerFpv.Input
{
    [Serializable]
    public class InputMap
    {
        public bool gamepad;
        public Dictionary<string, InputMapperItem> items = new Dictionary<string, InputMapperItem>();
        public InputMapperItem[] itemAry;

        public static void Save(string name, InputMap inputMap)
        {
            List<InputMapperItem> itemList = new List<InputMapperItem>(inputMap.items.Values);

            inputMap.itemAry = itemList.ToArray();

            string json = JsonUtility.ToJson(inputMap);

            PlayerPrefs.SetString(name, json);
        }

        public static InputMap Load(string name)
        {
            InputMap inputMap = null;

            string json = PlayerPrefs.GetString(name);

            if (json != string.Empty)
            {
                inputMap = JsonUtility.FromJson<InputMap>(json);

                inputMap.items = new Dictionary<string, InputMapperItem>();

                foreach (InputMapperItem i in inputMap.itemAry)
                {
                    inputMap.items.Add(i.axisKeyName, i);
                }
            }

            return inputMap;
        }
    }
}