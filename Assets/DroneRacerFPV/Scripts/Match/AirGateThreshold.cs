using DroneRacerFpv.Controller;
using System.Collections;
using UnityEngine;

namespace DroneRacerFpv.Match
{
    public class AirGateThreshold : MonoBehaviour
    {
        public AirGate airGate;

        public void OnTriggerEnter(Collider other)
        {
            //Debug.Log("OnTriggerEnter other=" + other.name);
            DroneRacer dr = other.gameObject.GetComponent<DroneRacer>();

            if (dr == DroneRacer.FindDroneRacer())
            {
                airGate.AirGateThresholdTriggered(dr);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            //Debug.Log("OnTriggerExit other=" + other.name);
            DroneRacer qr = other.gameObject.GetComponent<DroneRacer>();

            if (qr == DroneRacer.FindDroneRacer())
            {
                airGate.AirGateThresholdTriggered(qr);
            }
        }
    }
}