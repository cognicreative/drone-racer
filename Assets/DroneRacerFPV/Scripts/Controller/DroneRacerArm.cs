using DroneRacerFpv.Ui;
using UnityEngine;

namespace DroneRacerFpv.Controller
{
    [RequireComponent(typeof(DroneRacer))]
    public class DroneRacerArm : MonoBehaviour
    {
        public float deadZone = 0.05f;
        public bool disarmOnLostFocus = true;

        bool armed;

        DroneRacerJoystickInput input;

        DroneRacer droneRacer;

        void Awake()
        {
            input = GetComponent<DroneRacerJoystickInput>();
        }

        void Start()
        {
            droneRacer = GetComponent<DroneRacer>();
        }

        public bool autoArm = true;
        void Update()
        {
            if (autoArm == true)
            {
                if (armed == false && paused == false)
                {
                    //Debug.Log(string.Format("armed={0}  paused={0}", armed, paused));
                    if (IsAbleToArm() == true || input.Gamepad == true)
                    {
                        Arm();
                    }
                }
            }
        }

        bool IsAbleToArm()
        {
            bool retVal = false;

            //Debug.Log(string.Format("droneRacer.thrust={0}  droneRacer.yawForce={1}  droneRacer.pitchForce={2}  droneRacer.rollForce={3}", droneRacer.thrust, droneRacer.yawForce, droneRacer.pitchForce, droneRacer.rollForce));

            if (droneRacer.thrust <= 0 && droneRacer.yawForce <= 0 && droneRacer.pitchForce <= 0 && droneRacer.rollForce >= 0 && droneRacer.Crashed == false)
            {
                retVal = true;
            }

            return retVal;
        }

        bool paused;
        void OnApplicationFocus(bool hasFocus)
        {
            paused = !hasFocus;

            //Debug.Log("OnApplicationFocus  paused=" + paused);
            if (hasFocus == false && disarmOnLostFocus == true)
            {
                armed = false;
            }
        }

        public bool IsPaused()
        {
            return paused;
        }

        public void Pause()
        {
            //Debug.Log("Pause");
            paused = true;
        }

        public void Unpause()
        {
            //Debug.Log("Unpause");
            paused = false;
        }

        public bool Armed()
        {
            //Debug.Log(string.Format("Armed  armed={0}  paused={1}", armed, paused));
            return armed == true && paused == false;
        }

        public void Arm()
        {
            //Debug.Log("Arm");
            armed = true;
            UiManager.ShowArmed();
        }

        public void Disarm(bool showAlert, bool pause)
        {
            //Debug.Log("Disarm pause=" + pause);
            paused = pause;
            armed = false;
            if (showAlert == true && IsAbleToArm() == false)
            {
                UiManager.ShowDisarmed();
            }
        }

        public static DroneRacerArm FindDroneRacerArm()
        {
            DroneRacerArm dra = null;

            DroneRacer dr = DroneRacer.FindDroneRacer();

            if (dr != null)
            {
                dra = dr.GetComponent<DroneRacerArm>();
            }

            return dra;
        }
    }
}