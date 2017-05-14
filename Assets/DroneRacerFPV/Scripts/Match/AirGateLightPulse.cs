using UnityEngine;

namespace DroneRacerFpv.Match
{
    [RequireComponent(typeof(LensFlare))]
    public class AirGateLightPulse : MonoBehaviour
    {
        public float pulseSpeed = 25;

        float maxBrightness;
        LensFlare lensFlare;

        void Awake()
        {
            lensFlare = GetComponent<LensFlare>();
            maxBrightness = lensFlare.brightness;
        }

        void Update()
        {
            lensFlare.brightness = Mathf.PingPong(Time.time * pulseSpeed, maxBrightness);
        }

    }
}