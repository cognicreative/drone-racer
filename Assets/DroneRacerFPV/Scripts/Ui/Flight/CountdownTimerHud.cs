using DroneRacerFpv.Match;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DroneRacerFpv.Ui
{
    public class CountdownTimerHud : MonoBehaviour
    {
        static CountdownTimerHud instance;

        public Text text;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("CountdownTimerUi has already been created.");
            }

            Hide(true);
        }

        void Update()
        {
            if (showing == true)
            {
                TimeSpan ts = TimeSpan.FromSeconds(RaceManager.GetCountdownTimerTimeLeftSec());

                text.text = string.Format("{0:00}:{1:0}", ts.Seconds, ts.Milliseconds / 100);
            }
        }

        bool showing;

        public static void Hide(bool force = false)
        {
            if (instance != null)
            {
                if (instance.showing == true || force == true)
                {
                    instance.gameObject.SetActive(false);
                    instance.showing = false;
                }
            }
        }

        public static void Show()
        {
            if (instance != null)
            {
                if (instance.showing == false)
                {
                    instance.gameObject.SetActive(true);
                    instance.showing = true;
                }
            }
        }
    }
}