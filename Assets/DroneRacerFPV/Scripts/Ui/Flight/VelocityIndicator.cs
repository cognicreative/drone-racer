using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DroneRacerFpv.Utils;
using DroneRacerFpv.Controller;
using DroneRacerFpv.Settings;

namespace DroneRacerFpv.Ui
{
    public class VelocityIndicator : MonoBehaviour
    {
        VelocityRate velocityRate;

        public Text text;
        public Image background;
        public float updateFrequency = 0.1f;

        void Awake()
        {
            Hide();
        }

        bool showing;

        void Hide()
        {
            if (showing == true)
            {
                background.gameObject.SetActive(false);
                showing = false;
            }
        }

        void Show()
        {
            if (showing == false)
            {
                background.gameObject.SetActive(true);
                showing = true;
            }
        }

        DroneRacer droneRacer;

        public void OnEnable()
        {
            droneRacer = null;
            StartCoroutine(UpdateVelocity());
        }

        IEnumerator UpdateVelocity()
        {
            while (true)
            {
                if (droneRacer != null)
                {
                    velocityRate = (VelocityRate)PlayerPrefs.GetInt(Constants.VelocityIndicator);

                    switch (velocityRate)
                    {
                        case VelocityRate.KilometersPerHour:
                            Show();
                            text.text = string.Format("{0,3:0} KPH", droneRacer.kilometersPerHour);
                            break;
                        case VelocityRate.MilesPerHour:
                            Show();
                            text.text = string.Format("{0,3:0} MPH", droneRacer.milesPerHour);
                            break;
                        case VelocityRate.Hidden:
                            Hide();
                            break;
                    }
                }
                else
                {
                    droneRacer = DroneRacer.FindDroneRacer();
                }

                yield return new WaitForSeconds(updateFrequency);
            }
        }
    }
}