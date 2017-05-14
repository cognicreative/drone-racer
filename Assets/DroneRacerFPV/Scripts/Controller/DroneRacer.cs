using DroneRacerFpv.Match;
using DroneRacerFpv.Utils;
using System.Collections;
using UnityEngine;

namespace DroneRacerFpv.Controller
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(DroneRacerJoystickInput))]
    [RequireComponent(typeof(DroneRacerArm))]
    public class DroneRacer : MonoBehaviour
    {
        public float metersPerSecond;
        public float feetPerSecond;
        public float kilometersPerHour;
        public float milesPerHour;

        public float power = 5;
        public float responsiveness = 7.5f;
        public float yawSpeed = 3f;

        [HideInInspector]
        public float thrust;

        public float deadZone = 0.025f;

        float acroModeTiltSpeed = 3f;

        public bool autoLevel = true;
        float autoLevelModeTiltSpeed = 2f;

        [HideInInspector]
        public float yawForce;

        Rigidbody droneRigidbody;

        DroneRacerJoystickInput joystickInput;
        DroneRacerArm droneRacerArm;

        public float autoLevelStrength = 10f;

        Transform[] zeroThrustSpinPoint = new Transform[4];

        bool airMode;

        [HideInInspector]
        public float pitchForce;

        [HideInInspector]
        public float rollForce;

        void Initialize(Transform startHere)
        {
            UpdateStartPos(startHere.position);
            UpdateStartEulers(startHere.eulerAngles);

            Restart(false, RestartAt.RestartAtStartingPos);
        }

        void UpdateCameraAngle(Camera c)
        {
            float angle = PlayerPrefs.GetFloat(Constants.CameraAngle);

            Vector3 eulers = c.transform.localEulerAngles;
            eulers.x = -angle;
            c.transform.localEulerAngles = eulers;
        }

        void UpdateCameraFov(Camera c)
        {
            float fov = PlayerPrefs.GetFloat(Constants.CameraFov);

            c.fieldOfView = fov;
        }

        IEnumerator WaitStartPositionManager()
        {

            while (GameManager.IsReady() == false)
            {
                yield return null;
            }

        }

        void Start()
        {
            //Debug.Log("Start");

            zeroThrustSpinPoint[0] = SceneHelper.FindFirstChildInHierarchy(this, "FrontLeft").GetComponent<Transform>();
            zeroThrustSpinPoint[1] = SceneHelper.FindFirstChildInHierarchy(this, "FrontRight").GetComponent<Transform>();
            zeroThrustSpinPoint[2] = SceneHelper.FindFirstChildInHierarchy(this, "BackLeft").GetComponent<Transform>();
            zeroThrustSpinPoint[3] = SceneHelper.FindFirstChildInHierarchy(this, "BackRight").GetComponent<Transform>();

            joystickInput = GetComponent<DroneRacerJoystickInput>();
            droneRacerArm = GetComponent<DroneRacerArm>();
            droneRigidbody = GetComponent<Rigidbody>();

            UpdateSettings();

            Initialize(GameManager.GetStartPosition());
        }

        public static void UpdateSettings()
        {
            DroneRacer dr = FindDroneRacer();

            if (dr != null)
            {
                dr.power = PlayerPrefs.GetFloat(Constants.PowerOutput);

                dr.autoLevelStrength = PlayerPrefs.GetFloat(Constants.AutoLevelStrength);

                dr.autoLevel = PlayerPrefs.GetInt(Constants.AutoLevel) == 0 ? false : true;

                dr.altitudeHold = PlayerPrefs.GetInt(Constants.AltitudeHold) == 0 ? false : true;

                dr.altitudeHoldStrength = PlayerPrefs.GetFloat(Constants.AltitudeHoldStrength);

                dr.airMode = PlayerPrefs.GetInt(Constants.AirMode) == 0 ? false : true;

                dr.crashTolerance = PlayerPrefs.GetFloat(Constants.CrashTolerance);

                dr.detectCrash = PlayerPrefs.GetInt(Constants.DetectCrash) == 0 ? false : true;

                Camera c = SceneHelper.FindFirstChildInHierarchy<Camera>(dr, Constants.FpvCameraName);
                c.gameObject.SetActive(true);

                dr.UpdateCameraAngle(c);

                dr.UpdateCameraFov(c);

                DroneRacerJoystickInput.UpdateSettings();
            }
        }

        void Update()
        {
            thrust = joystickInput.leftStickY * responsiveness * power;
            //Debug.Log("thrust="+thrust);

            yawForce = MathHelper.DeadZoneClamp(joystickInput.leftStickX, deadZone) * yawSpeed * responsiveness;

            pitchForce = MathHelper.DeadZoneClamp(joystickInput.rightStickY, deadZone) * (autoLevel ? autoLevelModeTiltSpeed : acroModeTiltSpeed) * responsiveness;
            rollForce = -MathHelper.DeadZoneClamp(joystickInput.rightStickX, deadZone) * (autoLevel ? autoLevelModeTiltSpeed : acroModeTiltSpeed) * responsiveness;

            HandleButtons();
        }

        bool restartPressed;
        bool gotoStartingPositionPressed;
        void HandleButtons()
        {
            if (joystickInput.restartButton == true && restartPressed == false)
            {
                //Debug.Log("Restart pressed...");
                restartPressed = true;

                if (crashed == false)
                {
                    Restart(false, (RestartAt)PlayerPrefs.GetInt(Constants.RestartAt));
                }
            }
            else
            {
                restartPressed = false;
            }


            if (gotoStartingPositionPressed == false)
            {
                if (UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(KeyCode.RightControl))
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKey(KeyCode.RightAlt))
                    {
                        if (UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift))
                        {
                            if (UnityEngine.Input.GetKeyDown(KeyCode.S))
                            {

                                //Debug.Log("GotoStartingPosition pressed...");
                                gotoStartingPositionPressed = true;

                                Restart(false, RestartAt.RestartAtStartingPos);
                            }
                        }
                    }
                }
            }
            else
            {
                gotoStartingPositionPressed = false;
            }
        }

        Transform zeroThrustSpin1;
        Transform zeroThrustSpin2;

        float altitudeHoldThrustScalar = 0.5f;
        float altitudeHoldStrength = 50;

        bool altitudeHold;

        float noThrustFakeGravity = 1f;
        float crashingFakeGravity = 10f;

        void FixedUpdate()
        {
            metersPerSecond = droneRigidbody.velocity.magnitude / Constants.Scale;
            feetPerSecond = metersPerSecond * Constants.Meter2Foot;
            kilometersPerHour = metersPerSecond * Constants.MetersPerSec2KilometersPerHr;
            milesPerHour = metersPerSecond * Constants.MetersPerSec2MilesPerHr;

            if (crashed == false && (droneRacerArm.Armed() == true || joystickInput.Gamepad == true))
            {

                if (thrust > 0 || airMode == true)
                {
                    if (altitudeHold == true)
                    {
                        Ray ray = new Ray(transform.position, -Vector3.up);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, thrust))
                        {
                            float scaledThrust = thrust * altitudeHoldThrustScalar;
                            float proportionalHeight = Mathf.Max(0, (scaledThrust - hit.distance) / scaledThrust);
                            Vector3 appliedAltitudeHoldForce = Vector3.up * proportionalHeight * altitudeHoldStrength;
                            droneRigidbody.AddForce(appliedAltitudeHoldForce, ForceMode.Acceleration);
                        }
                    }

                    droneRigidbody.AddForce(transform.up * thrust);

                    droneRigidbody.AddRelativeTorque(0f, yawForce, 0f);
                    droneRigidbody.AddRelativeTorque(pitchForce, 0f, 0f);
                    droneRigidbody.AddRelativeTorque(0f, 0f, rollForce);


                    if (autoLevel == true)
                    {
                        Vector3 predictedUp = Quaternion.AngleAxis(
                                droneRigidbody.angularVelocity.magnitude * Time.deltaTime,
                                droneRigidbody.angularVelocity
                            ) * transform.up;

                        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);

                        float strength = autoLevelStrength * MathHelper.ScaleToRange(torqueVector.magnitude, 0, 1, joystickInput.minRotationRate, joystickInput.rotationRate);

                        droneRigidbody.AddTorque(torqueVector * strength);
                    }

                    zeroThrustSpin1 = null;
                    zeroThrustSpin2 = null;
                }
            }

            if (thrust <= 0 || crashed == true)
            {
                if (metersPerSecond > crashFinishedSpeed * 2)
                {
                    if (zeroThrustSpin1 == null)
                    {
                        zeroThrustSpin1 = zeroThrustSpinPoint[Random.Range(0, 4)];
                        zeroThrustSpin2 = zeroThrustSpinPoint[Random.Range(0, 4)];
                    }

                    droneRigidbody.AddTorque((zeroThrustSpin1.up + zeroThrustSpin2.up).normalized);


                    if (crashed == true)
                    {
                        //Debug.Log("crashedFakeGravity=" + crashedFakeGravity);
                        droneRigidbody.AddForce(-Vector3.up * crashingFakeGravity, ForceMode.Impulse);
                    }
                    else
                    {
                        //Debug.Log("noThrustFakeGravity=" + noThrustFakeGravity);
                        //Make drone fall faster if there is no thrust being applied
                        droneRigidbody.AddForce(-Vector3.up * noThrustFakeGravity, ForceMode.Impulse);
                    }

                }
            }
        }

        public float MotorSpeed
        {
            get
            {
                float motorSpeed = 0;

                if (thrust > 0 || airMode == true)
                {
                    float yawPower = Mathf.Abs(joystickInput.leftStickX);
                    float pitchPower = Mathf.Abs(joystickInput.rightStickY);
                    float rollPower = Mathf.Abs(joystickInput.rightStickX);

                    motorSpeed = joystickInput.powerLevel;

                    motorSpeed = Mathf.Max(motorSpeed, yawPower);
                    motorSpeed = Mathf.Max(motorSpeed, pitchPower);
                    motorSpeed = Mathf.Max(motorSpeed, rollPower);
                }

                return motorSpeed;
            }
        }

        bool crashed;
        public bool Crashed
        {
            get
            {
                return crashed;
            }
        }

        bool detectCrash;
        float crashTolerance;

        void OnCollisionEnter(Collision collision)
        {
            if (crashed == false && detectCrash == true && (collision.relativeVelocity.magnitude > crashTolerance || transform.up.y < 0.01f))
            {
                //Debug.Log(string.Format("Crashed:  crashTolerance={0}  magnitude={1}  transform.up={2}", crashTolerance, collision.relativeVelocity.magnitude, transform.up));

                //If this is a TrackBoundary then don't crash
                if (collision.gameObject.CompareTag("TrackBoundary") == false)
                {
                    crashed = true;

                    droneRacerArm.Disarm(false, true);

                    StartCoroutine(WaitForStopAfterCrash());
                }
            }
        }

        float crashFinishedSpeed = 2f;

        IEnumerator WaitForStopAfterCrash()
        {

            while (metersPerSecond > crashFinishedSpeed)
            {
                yield return null;
            }

            droneRacerArm.Disarm(true, false);

            Restart(PlayerPrefs.GetInt(Constants.AutoRestart) == 0 ? false : true, (RestartAt)PlayerPrefs.GetInt(Constants.RestartAt));
        }

        public void Restart(bool autoRestart, RestartAt restartAt)
        {
            //Debug.Log(string.Format("autoRestart={0}  autoRestartDelay={1}  restartAt={2}", autoRestart, RestartSettings.GetUnscaledAutoRestartDelay(), restartAt));
            StartCoroutine(HandleRestart(autoRestart, restartAt));
        }

        IEnumerator HandleRestart(bool autoRestart, RestartAt restartAt)
        {
            //Debug.Log(string.Format("HandleRestart  name={0}  isLocalPlayer={1}", gameObject.name, isLocalPlayer));

            if (autoRestart == true)
            {
                yield return new WaitForSeconds(PlayerPrefs.GetFloat(Constants.AutoRestartDelay));
            }

            PeformRestart(restartAt);
        }

        void CollidersEnable(bool enabled)
        {
            Collider[] colliders = GetComponents<Collider>();

            foreach (Collider c in colliders)
            {
                c.enabled = enabled;
            }
        }

        void PeformRestart(RestartAt restartAt)
        {
            //Debug.Log(string.Format("PeformRestart  name={0}  isLocalPlayer={1}", gameObject.name, isLocalPlayer));

            crashed = false;

            if (restartAt == RestartAt.RestartAtStartingPos)
            {
                transform.position = startPos;
                transform.eulerAngles = startEulers;
            }
            else
            {
                Vector3 eulers = transform.eulerAngles;

                eulers.z = 0;

                transform.eulerAngles = eulers;
            }

            thrust = 0;

            if (droneRigidbody != null)
            {
                //Default no velocity
                droneRigidbody.velocity = Vector3.zero;
                droneRigidbody.angularVelocity = Vector3.zero;
            }

        }

        RaceStatus raceStatus;
        public RaceStatus RaceStatus
        {
            get
            {
                return raceStatus;
            }

            set
            {
                raceStatus = value;
            }
        }

        Vector3 startPos;
        void UpdateStartPos(Vector3 pos)
        {
            startPos = pos;
            //Debug.Log("PerformUpdateStartPos startPos=" + startPos);
        }

        Vector3 startEulers;
        void UpdateStartEulers(Vector3 eulers)
        {
            startEulers = eulers;
            //Debug.Log("PerformUpdateStartEulers startEulers=" + startEulers);
        }

        public static DroneRacer FindDroneRacer()
        {
            return GameObject.FindObjectOfType<DroneRacer>();
        }

    }
}