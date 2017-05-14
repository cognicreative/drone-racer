using UnityEngine;

namespace DroneRacerFpv.Match
{
    public class AirGateLightSwitcher : MonoBehaviour
    {
        public AirGate airGate;

        public GameObject notCrossedGO;
        public GameObject crossedGO;
        public GameObject nextGO;

        AirGate.State lastState;

        void Update()
        {
            switch (airGate.GetState())
            {
                case AirGate.State.None:
                    HandleNone();
                    break;
                case AirGate.State.NotCrossed:
                    HandleNotCrossed();
                    break;
                case AirGate.State.Crossed:
                    HandleCrossed();
                    break;
                case AirGate.State.Next:
                    HandleNext();
                    break;
            }
        }

        void HandleNone()
        {
            if (airGate.GetState() != lastState)
            {
                lastState = airGate.GetState();

                nextGO.SetActive(false);
                crossedGO.SetActive(false);
                notCrossedGO.SetActive(false);
            }
        }

        void HandleNotCrossed()
        {
            if (airGate.GetState() != lastState)
            {
                lastState = airGate.GetState();

                nextGO.SetActive(false);
                crossedGO.SetActive(false);

                notCrossedGO.SetActive(true);
            }
        }

        void HandleCrossed()
        {
            if (airGate.GetState() != lastState)
            {
                lastState = airGate.GetState();

                nextGO.SetActive(false);
                notCrossedGO.SetActive(false);

                crossedGO.SetActive(true);
            }
        }

        void HandleNext()
        {
            if (airGate.GetState() != lastState)
            {
                lastState = airGate.GetState();

                crossedGO.SetActive(false);
                notCrossedGO.SetActive(false);

                nextGO.SetActive(true);
            }
        }
    }
}