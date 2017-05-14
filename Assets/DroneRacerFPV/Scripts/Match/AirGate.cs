using UnityEngine;

namespace DroneRacerFpv.Match
{
    public class AirGate : MonoBehaviour
    {
        public enum State { None, NotCrossed, Crossed, Next};

        State state;

        public State GetState()
        {
            return state;
        }

        public void SetState(State state)
        {
            this.state = state;
        }

        public void AirGateThresholdTriggered(Controller.DroneRacer droneRacer)
        {
            if (state == State.Next)
            {
                RaceManager.AirGateThresholdCrossed(droneRacer, this);
            }
        }
    }
}