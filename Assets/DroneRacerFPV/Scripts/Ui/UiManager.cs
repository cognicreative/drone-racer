using DroneRacerFpv.Input;
using DroneRacerFpv.Match;
using DroneRacerFpv.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DroneRacerFpv.Ui
{
    [RequireComponent(typeof(CalibrateJoystickSteps))]
    public class UiManager : MonoBehaviour
    {
        static UiManager instance;

        public Button raceButton;
        public Button closeRaceInfoHudButton;
        public RaceInfoHud raceInfoHud;

        public RectTransform flightPanel;
        public RectTransform startPanel;
        public RectTransform settingsPanel;
        public RectTransform calibrationPanel;

        RectTransform lastActivePanel;

        public Text armedIndicatorText;
        public RectTransform armedIndicatorBackground;

        CalibrateJoystickSteps calibrateJoystickSteps;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("UiManager has already been created.");
            }

            calibrateJoystickSteps = SceneHelper.FindFirstChildInHierarchy(this, "Calibrate").GetComponent<CalibrateJoystickSteps>();

            startPanel.gameObject.SetActive(true);
            flightPanel.gameObject.SetActive(false);
            settingsPanel.gameObject.SetActive(false);
            calibrationPanel.gameObject.SetActive(false);

            ResetFlightPanel();
        }

        void EnableLastActivePanel(bool nullify = false)
        {
            if (lastActivePanel != null)
            {
                lastActivePanel.gameObject.SetActive(true);

                if (nullify == true)
                {
                    lastActivePanel = null;
                }
            }
        }

        void DisableLastActivePanel(bool nullify = false)
        {
            if (lastActivePanel != null)
            {
                lastActivePanel.gameObject.SetActive(false);

                if (nullify == true)
                {
                    lastActivePanel = null;
                }
            }
        }

        #region FlightPanel
        void ResetFlightPanel()
        {
            GameManager.StopGame();
            RaceManager.StopRace();
            HideRaceInfoHud();
            CountdownTimerHud.Hide();
            HideArmedIndicator();
            raceButton.GetComponentInChildren<Text>().text = Constants.StartRace;
        }

        public void StopPlay()
        {
            ResetFlightPanel();

            startPanel.gameObject.SetActive(true);
            flightPanel.gameObject.SetActive(false);
        }

        public void Settings()
        {
            settingsPanel.gameObject.SetActive(true);
        }

        public void Race()
        {
            if (RaceManager.IsRaceInProgress() == true)
            {
                RaceManager.StopRace();
            }
            else if (RaceManager.IsGetReady() == false)
            {
                raceButton.GetComponentInChildren<Text>().text = Constants.GetReadyToRace;

                RaceManager.GetReadyToRace();

                CountdownTimerHud.Show();

                closeRaceInfoHudButton.gameObject.SetActive(false);
                raceInfoHud.gameObject.SetActive(true);
                raceInfoHud.StartWatchRaceStatus();

                StartCoroutine(WatchRaceStatus());
            }
        }

        IEnumerator WatchRaceStatus()
        {
            //Debug.Log("UiManager.WatchRaceStatus");

            while (RaceManager.IsRaceInProgress() == false)
            {
                yield return null;
            }

            CountdownTimerHud.Hide();

            raceButton.GetComponentInChildren<Text>().text = Constants.StopRace;
            raceInfoHud.StartWatchRaceStatus();

            while (RaceManager.IsRaceInProgress() == true)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (RaceManager.IsReady())
            {
                closeRaceInfoHudButton.gameObject.SetActive(true);
            }

            raceButton.GetComponentInChildren<Text>().text = Constants.StartRace;
        }

        public void HideRaceInfoHud()
        {
            closeRaceInfoHudButton.gameObject.SetActive(false);
            raceInfoHud.gameObject.SetActive(false);
        }

        #endregion

        #region StartPanel
        public void StartPlay()
        {
            GameManager.StartGame();
            flightPanel.gameObject.SetActive(true);
            startPanel.gameObject.SetActive(false);

            if (InputMap.Load(InputMapName.JoystickInputMap) == null && PlayerPrefs.GetInt(Constants.CalibrationChecked) == 0)
            {
                lastActivePanel = flightPanel;
                DisableLastActivePanel();
                Calibrate();
            }

        }

        public void Calibrate()
        {
            PlayerPrefs.SetInt(Constants.CalibrationChecked, 1);
            calibrationPanel.gameObject.SetActive(true);
            calibrateJoystickSteps.StartJoystickCalibration();
        }

        public void Exit()
        {
            if (Application.isEditor == true)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Application.Quit();
            }
        }
        #endregion

        #region CalibrationPanel
        public void CalibrationClose()
        {
            StartCoroutine(HandleCalibrationClose());
        }

        IEnumerator HandleCalibrationClose()
        {
            calibrateJoystickSteps.StopJoystickCalibration();

            while (calibrateJoystickSteps.IsAxisDetectionActive() == true)
            {
                yield return null;
            }

            EnableLastActivePanel(true);

            yield return null;

            calibrationPanel.gameObject.SetActive(false);
        }
        #endregion

        #region ArmedIndicator

        public static void HideArmedIndicator()
        {
            if (instance != null)
            {
                instance.armedIndicatorBackground.gameObject.SetActive(false);
            }
        }

        public static void ShowArmedIndicator()
        {
            if (instance != null)
            {
                instance.armedIndicatorBackground.gameObject.SetActive(true);
            }
        }

        public static void ShowArmed()
        {
            if (instance != null)
            {
                ShowArmedIndicator();
                instance.armedIndicatorText.text = Constants.Armed;
            }
        }

        public static void ShowDisarmed()
        {
            if (instance != null)
            {
                ShowArmedIndicator();
                instance.armedIndicatorText.text = Constants.Disarmed;
            }
        }

        #endregion


    }
}