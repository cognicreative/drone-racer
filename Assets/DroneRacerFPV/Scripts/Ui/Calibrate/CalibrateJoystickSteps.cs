using UnityEngine;
using DroneRacerFpv.Input;
using UnityEngine.UI;
using DroneRacerFpv.Utils;
using System.Collections;
using DroneRacerFpv.Controller;

namespace DroneRacerFpv.Ui
{
    [RequireComponent(typeof(InputMapper))]
    public class CalibrateJoystickSteps : MonoBehaviour
    {
        public enum Step
        {
            None,
            Yaw,
            Throttle,
            Roll,
            Pitch,
        }

        public RectTransform joystickCalibrationSteps;

        public RectTransform moveLeftJoystickLeftRight;
        public RectTransform moveLeftJoystickUpDown;
        public RectTransform moveRightJoystickLeftRight;
        public RectTransform moveRightJoystickUpDown;

        public JoystickThumbMover leftThumbMover;
        public JoystickThumbMover rightThumbMover;

        ControlSlider trimVerticalLeftSlider;
        ControlSlider scaleVerticalLeftSlider;
        ControlSlider trimHorizontalLeftSlider;
        ControlSlider scaleHorizontalLeftSlider;
        ControlSlider deadzoneHorizontalLeftSlider;
        ControlSlider deadzoneVerticalLeftSlider;

        ControlSlider trimVerticalRightSlider;
        ControlSlider scaleVerticalRightSlider;
        ControlSlider trimHorizontalRightSlider;
        ControlSlider scaleHorizontalRightSlider;
        ControlSlider deadzoneHorizontalRightSlider;
        ControlSlider deadzoneVerticalRightSlider;

        InputMapper inputMapper;

        InputMap inputMap;

        Toggle leftJoystickLeftRightInvert;
        Toggle leftJoystickUpDownInvert;
        Toggle rightJoystickLeftRightInvert;
        Toggle rightJoystickUpDownInvert;

        public Toggle gamepadToggle;

        DroneRacerArm droneRacerArm;

        void Awake()
        {
            inputMapper = GetComponent<InputMapper>();

            moveLeftJoystickLeftRight.gameObject.SetActive(false);

            SceneHelper.FindFirstChildInHierarchy(moveLeftJoystickLeftRight, "Reset").GetComponent<Button>().onClick.AddListener(ResetCalibration);

            leftJoystickLeftRightInvert = SceneHelper.FindFirstChildInHierarchy(moveLeftJoystickLeftRight, "InvertToggle").GetComponent<Toggle>();
            leftJoystickLeftRightInvert.onValueChanged.AddListener(ToggleInvert);


            SceneHelper.FindFirstChildInHierarchy(moveLeftJoystickLeftRight, "Next").GetComponent<Button>().onClick.AddListener(CalibrateYawNext);


            moveLeftJoystickUpDown.gameObject.SetActive(false);

            SceneHelper.FindFirstChildInHierarchy(moveLeftJoystickUpDown, "Back").GetComponent<Button>().onClick.AddListener(CalibrateThrottleBack);

            SceneHelper.FindFirstChildInHierarchy(moveLeftJoystickUpDown, "Reset").GetComponent<Button>().onClick.AddListener(ResetCalibration);

            leftJoystickUpDownInvert = SceneHelper.FindFirstChildInHierarchy(moveLeftJoystickUpDown, "InvertToggle").GetComponent<Toggle>();
            leftJoystickUpDownInvert.onValueChanged.AddListener(ToggleInvert);

            SceneHelper.FindFirstChildInHierarchy(moveLeftJoystickUpDown, "Next").GetComponent<Button>().onClick.AddListener(CalibrateThrottleNext);


            moveRightJoystickLeftRight.gameObject.SetActive(false);

            SceneHelper.FindFirstChildInHierarchy(moveRightJoystickLeftRight, "Back").GetComponent<Button>().onClick.AddListener(CalibrateRollBack);

            SceneHelper.FindFirstChildInHierarchy(moveRightJoystickLeftRight, "Reset").GetComponent<Button>().onClick.AddListener(ResetCalibration);

            rightJoystickLeftRightInvert = SceneHelper.FindFirstChildInHierarchy(moveRightJoystickLeftRight, "InvertToggle").GetComponent<Toggle>();
            rightJoystickLeftRightInvert.onValueChanged.AddListener(ToggleInvert);

            SceneHelper.FindFirstChildInHierarchy(moveRightJoystickLeftRight, "Next").GetComponent<Button>().onClick.AddListener(CalibrateRollNext);


            moveRightJoystickUpDown.gameObject.SetActive(false);

            SceneHelper.FindFirstChildInHierarchy(moveRightJoystickUpDown, "Back").GetComponent<Button>().onClick.AddListener(CalibratePitchBack);

            SceneHelper.FindFirstChildInHierarchy(moveRightJoystickUpDown, "Reset").GetComponent<Button>().onClick.AddListener(ResetCalibration);

            rightJoystickUpDownInvert = SceneHelper.FindFirstChildInHierarchy(moveRightJoystickUpDown, "InvertToggle").GetComponent<Toggle>();
            rightJoystickUpDownInvert.onValueChanged.AddListener(ToggleInvert);

            gamepadToggle.onValueChanged.AddListener(ToggleGamepad);

            SceneHelper.FindFirstChildInHierarchy(moveRightJoystickUpDown, "Finish").GetComponent<Button>().onClick.AddListener(CalibratePitchFinish);

            trimVerticalLeftSlider = SceneHelper.FindFirstChildInHierarchy(this, "TrimVerticalLeftSlider").GetComponent<ControlSlider>();
            scaleVerticalLeftSlider = SceneHelper.FindFirstChildInHierarchy(this, "ScaleVerticalLeftSlider").GetComponent<ControlSlider>();
            trimHorizontalLeftSlider = SceneHelper.FindFirstChildInHierarchy(this, "TrimHorizontalLeftSlider").GetComponent<ControlSlider>();
            scaleHorizontalLeftSlider = SceneHelper.FindFirstChildInHierarchy(this, "ScaleHorizontalLeftSlider").GetComponent<ControlSlider>();
            deadzoneHorizontalLeftSlider = SceneHelper.FindFirstChildInHierarchy(this, "DeadzoneHorizontalLeftSlider").GetComponent<ControlSlider>();
            deadzoneVerticalLeftSlider = SceneHelper.FindFirstChildInHierarchy(this, "DeadzoneVerticalLeftSlider").GetComponent<ControlSlider>();

            trimVerticalRightSlider = SceneHelper.FindFirstChildInHierarchy(this, "TrimVerticalRightSlider").GetComponent<ControlSlider>();
            scaleVerticalRightSlider = SceneHelper.FindFirstChildInHierarchy(this, "ScaleVerticalRightSlider").GetComponent<ControlSlider>();
            trimHorizontalRightSlider = SceneHelper.FindFirstChildInHierarchy(this, "TrimHorizontalRightSlider").GetComponent<ControlSlider>();
            scaleHorizontalRightSlider = SceneHelper.FindFirstChildInHierarchy(this, "ScaleHorizontalRightSlider").GetComponent<ControlSlider>();
            deadzoneHorizontalRightSlider = SceneHelper.FindFirstChildInHierarchy(this, "DeadzoneHorizontalRightSlider").GetComponent<ControlSlider>();
            deadzoneVerticalRightSlider = SceneHelper.FindFirstChildInHierarchy(this, "DeadzoneVerticalRightSlider").GetComponent<ControlSlider>();
        }

        public void StartJoystickCalibration()
        {
            //Debug.Log("StartJoystickCalibration()");

            droneRacerArm = DroneRacerArm.FindDroneRacerArm();

            inputMap = InputMap.Load(InputMapName.JoystickInputMap);

            if (inputMap == null)
            {
                inputMap = new InputMap();
            }
            else
            {
                UpdateUi();
            }

            DisableStepDialogs();

            joystickCalibrationSteps.gameObject.SetActive(true);

            currentStep = Step.None;

            StartCalibrateYaw();
        }

        void DisableDroneRacerInput()
        {
            if (droneRacerArm != null)
            {
                //Debug.Log("DisableDroneRacerInput()");

                droneRacerArm.Disarm(false, true);
            }
        }

        void EnableDroneRacerInput()
        {
            if (droneRacerArm != null)
            {
                DroneRacerJoystickInput drji = DroneRacerJoystickInput.FindDroneRacerJoystickInput();

                if (drji != null)
                {
                    //Debug.Log("StopJoystickCalibration  drji.UpdateJoystickInputMap()");

                    drji.UpdateJoystickInputMap();
                }

                //Debug.Log("StopJoystickCalibration  droneRacerArm.Unpause()");

                droneRacerArm.Unpause();
            }
        }

        public void StopJoystickCalibration()
        {
            StopAxisDetection();

            DisableStepDialogs();

            EnableDroneRacerInput();
        }

        void UpdateUi()
        {
            foreach (InputMapperItem i in inputMap.itemAry)
            {
                if (i.axisKeyName == InputNames.Yaw)
                {
                    trimHorizontalLeftSlider.SetValue(i.trim);
                    scaleHorizontalLeftSlider.SetValue(i.scale);
                    deadzoneHorizontalLeftSlider.SetValue(i.deadzone);

                    leftJoystickLeftRightInvert.isOn = i.invertValue == -1;
                }
                else if (i.axisKeyName == InputNames.Throttle)
                {
                    trimVerticalLeftSlider.SetValue(i.trim);
                    scaleVerticalLeftSlider.SetValue(i.scale);
                    deadzoneVerticalLeftSlider.SetValue(i.deadzone);

                    leftJoystickUpDownInvert.isOn = i.invertValue == -1;
                }
                else if (i.axisKeyName == InputNames.Pitch)
                {
                    trimVerticalRightSlider.SetValue(i.trim);
                    scaleVerticalRightSlider.SetValue(i.scale);
                    deadzoneVerticalRightSlider.SetValue(i.deadzone);

                    rightJoystickUpDownInvert.isOn = i.invertValue == -1;
                }
                else if (i.axisKeyName == InputNames.Roll)
                {
                    trimHorizontalRightSlider.SetValue(i.trim);
                    scaleHorizontalRightSlider.SetValue(i.scale);
                    deadzoneHorizontalRightSlider.SetValue(i.deadzone);

                    rightJoystickLeftRightInvert.isOn = i.invertValue == -1;
                }
            }

            gamepadToggle.isOn = inputMap.gamepad;
        }

        void DisableStepDialogs()
        {
            if (moveLeftJoystickLeftRight.gameObject.activeSelf == true)
            {
                moveLeftJoystickLeftRight.gameObject.SetActive(false);
            }

            if (moveLeftJoystickUpDown.gameObject.activeSelf == true)
            {
                moveLeftJoystickUpDown.gameObject.SetActive(false);
            }

            if (moveRightJoystickLeftRight.gameObject.activeSelf == true)
            {
                moveRightJoystickLeftRight.gameObject.SetActive(false);
            }

            if (moveRightJoystickUpDown.gameObject.activeSelf == true)
            {
                moveRightJoystickUpDown.gameObject.SetActive(false);
            }
        }

        public void ToggleGamepad(bool value)
        {
            inputMap.gamepad = value;
        }

        bool axisDetectionActive;
        public bool IsAxisDetectionActive()
        {
            return axisDetectionActive;
        }

        bool cancelAxisDetection;

        float updateFreq = 0.25f;

        void StartAxisDetection(IEnumerator e)
        {
            StopAxisDetection();
            StartCoroutine(WaitForCurrentAxisDetectionCancel(e));
        }

        IEnumerator WaitForCurrentAxisDetectionCancel(IEnumerator e)
        {
            while (axisDetectionActive == true)
            {
                yield return null;
            }

            cancelAxisDetection = false;
            axisDetectionActive = true;

            StartCoroutine(e);
        }

        void StopAxisDetection()
        {
            cancelAxisDetection = true;
        }

        void UpdateInputMap(Step step, InputMapperItem m)
        {
            string axisKeyName = null;

            switch (step)
            {
                case Step.Yaw:
                    axisKeyName = InputNames.Yaw;
                    break;
                case Step.Throttle:
                    axisKeyName = InputNames.Throttle;
                    break;
                case Step.Roll:
                    axisKeyName = InputNames.Roll;
                    break;
                case Step.Pitch:
                    axisKeyName = InputNames.Pitch;
                    break;
            }

            if (axisKeyName != null)
            {
                m.axisKeyName = axisKeyName;

                InputMapperItem cur;
                if (inputMap.items.TryGetValue(axisKeyName, out cur))
                {
                    inputMap.items[axisKeyName] = new InputMapperItem(m);
                }
                else
                {
                    inputMap.items.Add(axisKeyName, new InputMapperItem(m));
                }
            }
        }

        float invertValue = 1;

        void ResetJoystickThumbs()
        {
            leftThumbMover.ResetPosition();
            rightThumbMover.ResetPosition();
        }

        public void ToggleInvert(bool invert)
        {
            invertValue = invert ? -1 : 1;
        }

        IEnumerator DetectAxis(Step step)
        {
            inputMapper.Init();

            ResetJoystickThumbs();

            InputMapperItem m;

            switch (step)
            {
                case Step.Yaw:
                    invertValue = leftJoystickLeftRightInvert.isOn ? -1 : 1;
                    break;
                case Step.Throttle:
                    invertValue = leftJoystickUpDownInvert.isOn ? -1 : 1;
                    break;
                case Step.Roll:
                    invertValue = rightJoystickLeftRightInvert.isOn ? -1 : 1;
                    break;
                case Step.Pitch:
                    invertValue = rightJoystickUpDownInvert.isOn ? -1 : 1;
                    break;
            }

            while (cancelAxisDetection == false)
            {
                m = inputMapper.SelectActiveAxis();

                if (m != null)
                {
                    m.invertValue = invertValue;

                    switch (step)
                    {
                        case Step.Yaw:
                            m.trim = trimHorizontalLeftSlider.GetValue();
                            m.scale = scaleHorizontalLeftSlider.GetValue();
                            m.deadzone = deadzoneHorizontalLeftSlider.GetValue();
                            break;
                        case Step.Throttle:
                            m.trim = trimVerticalLeftSlider.GetValue();
                            m.scale = scaleVerticalLeftSlider.GetValue();
                            m.deadzone = deadzoneVerticalLeftSlider.GetValue();
                            break;
                        case Step.Roll:
                            m.trim = trimHorizontalRightSlider.GetValue();
                            m.scale = scaleHorizontalRightSlider.GetValue();
                            m.deadzone = deadzoneHorizontalRightSlider.GetValue();
                            break;
                        case Step.Pitch:
                            m.trim = trimVerticalRightSlider.GetValue();
                            m.scale = scaleVerticalRightSlider.GetValue();
                            m.deadzone = deadzoneVerticalRightSlider.GetValue();
                            break;
                    }

                    UpdateInputMap(step, m);

                    float v = InputHelper.GetInputAxis(m.axisName);

                    //Debug.Log(string.Format("m.invertValue={0}", m.invertValue));

                    //Debug.Log(string.Format("InputHelper.GetInputAxis(m.axisName)  m.axisName={0} raw v={1}", m.axisName, v));

                    v = m.ApplyInvertTrimScale(v);

                    //Debug.Log(string.Format("InputHelper.GetInputAxis(m.axisName)  m.axisName={0} m.trim={1} m.scale={2} v={3}", m.axisName, m.trim, m.scale, v));

                    switch (step)
                    {
                        case Step.Yaw:
                            leftThumbMover.LeftRight(v, m.minValue, m.maxValue);
                            break;
                        case Step.Throttle:
                            leftThumbMover.UpDown(v, m.minValue, m.maxValue);
                            break;
                        case Step.Roll:
                            rightThumbMover.LeftRight(v, m.minValue, m.maxValue);
                            break;
                        case Step.Pitch:
                            rightThumbMover.UpDown(v, m.minValue, m.maxValue);
                            break;
                    }
                }

                DisableDroneRacerInput();

                yield return new WaitForSeconds(updateFreq);
            }

            axisDetectionActive = false;

            //Debug.Log("DetectAxis() end");
        }

        public void ResetCalibration()
        {
            StartAxisDetection(DetectAxis(currentStep));
        }

        Step currentStep = Step.None;

        public void StartCalibrateYaw()
        {
            moveLeftJoystickLeftRight.gameObject.SetActive(true);

            currentStep = Step.Yaw;
            StartAxisDetection(DetectAxis(Step.Yaw));
        }

        public void CalibrateYawNext()
        {
            moveLeftJoystickLeftRight.gameObject.SetActive(false);
            moveLeftJoystickUpDown.gameObject.SetActive(true);

            currentStep = Step.Throttle;
            StartAxisDetection(DetectAxis(Step.Throttle));
        }

        public void CalibrateThrottleBack()
        {
            moveLeftJoystickUpDown.gameObject.SetActive(false);
            moveLeftJoystickLeftRight.gameObject.SetActive(true);

            currentStep = Step.Yaw;
            StartAxisDetection(DetectAxis(Step.Yaw));
        }

        public void CalibrateThrottleNext()
        {
            moveLeftJoystickUpDown.gameObject.SetActive(false);
            moveRightJoystickLeftRight.gameObject.SetActive(true);

            currentStep = Step.Roll;
            StartAxisDetection(DetectAxis(Step.Roll));
        }

        public void CalibrateRollBack()
        {
            moveRightJoystickLeftRight.gameObject.SetActive(false);
            moveLeftJoystickUpDown.gameObject.SetActive(true);

            currentStep = Step.Throttle;
            StartAxisDetection(DetectAxis(Step.Throttle));
        }

        public void CalibrateRollNext()
        {
            moveRightJoystickLeftRight.gameObject.SetActive(false);
            moveRightJoystickUpDown.gameObject.SetActive(true);

            currentStep = Step.Pitch;
            StartAxisDetection(DetectAxis(Step.Pitch));
        }

        public void CalibratePitchBack()
        {
            moveRightJoystickUpDown.gameObject.SetActive(false);
            moveRightJoystickLeftRight.gameObject.SetActive(true);

            currentStep = Step.Roll;
            StartAxisDetection(DetectAxis(Step.Roll));
        }

        public void CalibratePitchFinish()
        {
            currentStep = Step.None;

            moveRightJoystickUpDown.gameObject.SetActive(false);

            StopAxisDetection();

            InputMap.Save(InputMapName.JoystickInputMap, inputMap);
        }

        public void StartScanningJoysticks()
        {
            inputMapper.enabled = true;
        }

        public void StopScanningJoysticks()
        {
            if (inputMapper != null)
            {
                inputMapper.enabled = false;
            }
        }
    }
}
