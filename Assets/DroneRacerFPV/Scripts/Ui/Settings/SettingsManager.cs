using DroneRacerFpv.Controller;
using UnityEngine;

namespace DroneRacerFpv.Ui
{
    public class SettingsManager : MonoBehaviour
    {
        static SettingsManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("SettingsManager has already been created.");
            }
        }

        public void SettingsClose()
        {
            gameObject.SetActive(false);
            DroneRacer.UpdateSettings();
        }

    }
}