using UnityEngine;
using DroneRacerFpv.Input;
using DroneRacerFpv.Utils;

namespace DroneRacerFpv.Controller
{
    [RequireComponent(typeof(DroneRacer))]
    public class DroneRacerJoystickInput : MonoBehaviour
    {
        InputMap joystickInputMap;
        InputMapperItem yaw;
        InputMapperItem throttle;
        InputMapperItem roll;
        InputMapperItem pitch;

        bool gamepad;
        public bool Gamepad
        {
            get
            {
                return gamepad;
            }
        }

        void Start()
        {
            UpdateJoystickInputMap();
        }

        public void UpdateJoystickInputMap()
        {
            joystickInputMap = InputMap.Load(InputMapName.JoystickInputMap);

            if (joystickInputMap != null)
            {
                gamepad = joystickInputMap.gamepad;

				joystickInputMap.items.TryGetValue (InputNames.Yaw, out yaw);
//                yaw = joystickInputMap.items[InputNames.Yaw];

				joystickInputMap.items.TryGetValue (InputNames.Throttle, out throttle);
//                throttle = joystickInputMap.items[InputNames.Throttle];

				joystickInputMap.items.TryGetValue (InputNames.Roll, out roll);
//				roll = joystickInputMap.items[InputNames.Roll];

				joystickInputMap.items.TryGetValue (InputNames.Pitch, out pitch);
//				pitch = joystickInputMap.items[InputNames.Pitch];
            }
        }

        void OnRotationRateChanged(float value)
        {
            rotationRate = value;
            halfRotationRate = rotationRate / 2;
        }

        void OnExponentialRateChanged(bool value)
        {
            exponentialRate = value;
        }

        void OnExponentialRateFactorChanged(float value)
        {
            exponentialRateFactor = value;
        }

        void OnThrottleChangeRateChanged(float value)
        {
            throttleChangeRate = value;
        }

        void Update()
        {
            GetInput();
        }

        [HideInInspector]
        public float leftStickX = 0;                             //Used for the horizontal movement of the left thumbstick
        float lastLeftStickX = 0;

        [HideInInspector]
        public float leftStickY = 0;                             //Used for the vertical movement of the left thumbstick
        float lastLeftStickY = 0;

        public float gamepadMinValue = -0.01f;
        public float gamepadMaxValue = 0.01f;

        float gamepadValue = 0;

        [HideInInspector]
        public float throttleChangeRate = 100f;
        [HideInInspector]
        public float powerLevel = 0;

        [HideInInspector]
        public float rightStickX = 0;                            //Used for the horizontal movement of the right thumbstick
        float lastRightStickX = 0;

        [HideInInspector]
        public float rightStickY = 0;                            //Used for the vertical movement of the left thumbstick
        float lastRightStickY = 0;

        [HideInInspector]
        public bool restartButton;

        public float minAxisVertLeft = 0.25f;
        public float maxAxisVertLeft = 5f;

        public float rotationRate = 1f;
        float halfRotationRate;

        [HideInInspector]
        public float minRotationRate = 0.5f;

        [HideInInspector]
        public float maxRotationRate = 2;

        bool exponentialRate;
        float exponentialRateFactor;

        float RawThrottle(bool joystickConnected)
        {
            float retVal = 0;

            if (joystickConnected == true)
            {
                retVal = InputHelper.GetInputAxis(throttle.axisName);

                //Debug.Log("RawThrottle retVal=" + retVal);

            }
            else
            {
                retVal = InputHelper.GetInputAxis(InputNames.DesktopThrottleAxis);
            }


            return retVal;
        }

        float RawYaw(bool joystickConnected)
        {
            float retVal = 0;

            if (joystickConnected == true)
            {
                retVal = InputHelper.GetInputAxis(yaw.axisName);
            }
            else
            {
                retVal = InputHelper.GetInputAxis(InputNames.DesktopYawAxis);
            }

            return retVal;
        }

        float RawPitch(bool joystickConnected)
        {
            float retVal = 0;

            if (joystickConnected == true)
            {
                retVal = InputHelper.GetInputAxis(pitch.axisName);
            }
            else
            {
                retVal = InputHelper.GetInputAxis(InputNames.DesktopPitchAxis);
            }

            return retVal;
        }

        float RawRoll(bool joystickConnected)
        {
            float retVal = 0;

            if (joystickConnected == true)
            {
                retVal = InputHelper.GetInputAxis(roll.axisName);
            }
            else
            {
                retVal = InputHelper.GetInputAxis(InputNames.DesktopRollAxis);
            }

            return retVal;
        }

        void GetInput()
        {
            bool joystickConnected = InputHelper.JoystickConnected() == true && joystickInputMap != null;

            if (joystickConnected == true)
            {
                leftStickX = MathHelper.ValueChangeInDeadZone(RawYaw(joystickConnected), lastLeftStickX, yaw.deadzone);

                lastLeftStickX = leftStickX;

                leftStickX = yaw.ApplyInvertTrimScale(leftStickX);
                leftStickX = MathHelper.ScaleToRange(leftStickX, yaw.minValue, yaw.maxValue, -halfRotationRate, halfRotationRate, true, exponentialRate, exponentialRateFactor);
            }
            else
            {
                leftStickX = MathHelper.ValueChangeInDeadZone(RawYaw(joystickConnected), lastLeftStickX, 0);

                lastLeftStickX = leftStickX;

                //Keyboard value is -1 to 1
                leftStickX = MathHelper.ScaleToRange(leftStickX, -1, 1, -halfRotationRate, halfRotationRate, true, exponentialRate, exponentialRateFactor);
                lastLeftStickX = leftStickX;
            }

            //Debug.Log("leftStickX=" + leftStickX);

            float axisValue;

            if (gamepad == false || joystickConnected == false)
            {
                axisValue = MathHelper.ValueChangeInDeadZone(RawThrottle(joystickConnected), lastLeftStickY, 0);

                lastLeftStickY = axisValue;

                if (joystickConnected == true)
                {
                    axisValue = throttle.ApplyInvertTrimScale(axisValue);
                    powerLevel = Mathf.Lerp(powerLevel, MathHelper.ScaleToRange(axisValue, throttle.minValue, throttle.maxValue, 0, 1), Time.deltaTime * throttleChangeRate);
                }
                else
                {
                    powerLevel = Mathf.Lerp(powerLevel, axisValue, Time.deltaTime * throttleChangeRate);
                }

                leftStickY = MathHelper.ScaleToRange(powerLevel, 0, 1, minAxisVertLeft, maxAxisVertLeft);

                //Debug.Log("leftStickY=" + leftStickY);
            }
            else
            {
                axisValue = MathHelper.DeadZoneClamp(RawThrottle(joystickConnected), throttle.deadzone);

                axisValue = throttle.ApplyInvertTrimScale(axisValue);

                gamepadValue = MathHelper.ScaleToRange(axisValue, throttle.minValue, throttle.maxValue, gamepadMinValue, gamepadMaxValue);

                leftStickY += gamepadValue;

                leftStickY = Mathf.Clamp(leftStickY, minAxisVertLeft, maxAxisVertLeft);

                powerLevel = MathHelper.ScaleToRange(leftStickY, minAxisVertLeft, maxAxisVertLeft, 0, 1);

                //Debug.Log(string.Format("{0}  {1}  {2}  {3}", axisValue, gamepadValue, leftStickY, powerLevel));
            }

            if (joystickConnected == true)
            {
                rightStickX = MathHelper.ValueChangeInDeadZone(RawRoll(joystickConnected), lastRightStickX, roll.deadzone);
                rightStickX = roll.ApplyInvertTrimScale(rightStickX);
                rightStickX = MathHelper.ScaleToRange(rightStickX, roll.minValue, roll.maxValue, -halfRotationRate, halfRotationRate, true, exponentialRate, exponentialRateFactor);
            }
            else
            {
                rightStickX = MathHelper.ValueChangeInDeadZone(RawRoll(joystickConnected), lastRightStickX, 0);
                rightStickX = MathHelper.ScaleToRange(rightStickX, -1, 1, -halfRotationRate, halfRotationRate, true, exponentialRate, exponentialRateFactor);
            }

            if (joystickConnected == true)
            {
                rightStickY = MathHelper.ValueChangeInDeadZone(RawPitch(joystickConnected), lastRightStickY, pitch.deadzone);
                rightStickY = pitch.ApplyInvertTrimScale(rightStickY);
                rightStickY = MathHelper.ScaleToRange(rightStickY, pitch.minValue, pitch.maxValue, -halfRotationRate, halfRotationRate, true, exponentialRate, exponentialRateFactor);
            }
            else
            {
                rightStickY = MathHelper.ValueChangeInDeadZone(RawPitch(joystickConnected), lastRightStickY, 0);
                rightStickY = MathHelper.ScaleToRange(rightStickY, -1, 1, -halfRotationRate, halfRotationRate, true, exponentialRate, exponentialRateFactor);
            }

            //Debug.Log(string.Format("leftStickX={0}  leftStickY={1}  rightStickX={2}  rightStickY={3}  powerLevel={4}", lastLeftStickX, lastLeftStickY, rightStickX, rightStickY, powerLevel));

            restartButton = InputHelper.GetButtonDown("Restart");
        }

        public static void UpdateSettings()
        {
            DroneRacer dr = DroneRacer.FindDroneRacer();

            if (dr != null)
            {
                DroneRacerJoystickInput drji = dr.gameObject.GetComponent<DroneRacerJoystickInput>();

                drji.rotationRate = PlayerPrefs.GetFloat(Constants.RotationRate);
                drji.halfRotationRate = drji.rotationRate / 2;

                drji.exponentialRate = PlayerPrefs.GetInt(Constants.ExponentialRate) == 0 ? false : true;

                drji.exponentialRateFactor = PlayerPrefs.GetFloat(Constants.ExponentialRateFactor);

                drji.throttleChangeRate = PlayerPrefs.GetFloat(Constants.ThrottleChangeRate);
            }
        }

        public static DroneRacerJoystickInput FindDroneRacerJoystickInput()
        {
            DroneRacerJoystickInput drji = null;

            DroneRacer dr = DroneRacer.FindDroneRacer();

            if (dr != null)
            {
                drji = dr.GetComponent<DroneRacerJoystickInput>();
            }

            return drji;
        }

    }
}