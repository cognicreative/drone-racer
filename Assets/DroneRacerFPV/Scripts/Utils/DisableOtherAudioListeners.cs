using UnityEngine;

namespace DroneRacerFpv.Utils
{
    public class DisableOtherAudioListeners : MonoBehaviour
    {
        public bool enableMe;

        void Awake()
        {
            AudioListener[] audioListeners = GameObject.FindObjectsOfType<AudioListener>();

            foreach(AudioListener a in audioListeners)
            {
                if (gameObject == a.gameObject)
                {
                    a.enabled = true;
                }
                else
                {
                    a.enabled = false;
                }
            }
        }
    }
}