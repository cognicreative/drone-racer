using DroneRacerFpv.Controller;
using DroneRacerFpv.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DroneRacerFpv.Match
{
    [RequireComponent(typeof(Stopwatch))]
    [RequireComponent(typeof(CountdownTimer))]
    public class RaceManager : MonoBehaviour
    {
        static RaceManager instance;

        List<AirGate> airGates = new List<AirGate>();

        DroneRacer droneRacer;
        DroneRacerArm droneRacerArm;

        int curAirGateIdx;
        int lapCount;

        RaceInfo raceInfo;
        RaceInfo localRaceInfo;

        Stopwatch stopwatch;
        CountdownTimer countdownTimer;

        bool raceManagerIsReady;
        bool initializing;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("RaceManager has already been created.");
            }

            stopwatch = GetComponent<Stopwatch>();

            countdownTimer = GetComponent<CountdownTimer>();

            countdownTimer.AddTimerFinishedListener(CountdownTimerFinished);
        }

        void Update()
        {
            if (droneRacer == null && initializing == false)
            {
                initializing = true;
                raceManagerIsReady = false;
                StartCoroutine(InvokeInit());
            }
        }

        void InitLocalRaceData()
        {
            localRaceInfo = new RaceInfo(
                (int)PlayerPrefs.GetFloat(Constants.TotalLaps),
                PlayerPrefs.GetFloat(Constants.CountdownTimeSec),
                PlayerPrefs.GetInt(Constants.DetectCrash) == 0 ? false : true,
                PlayerPrefs.GetFloat(Constants.CrashTolerance),
                (RestartAt)PlayerPrefs.GetInt(Constants.RestartAt)
                );

            raceInfo = new RaceInfo(localRaceInfo);

            curAirGateIdx = -1;
            lapCount = 0;

            firstThresholdCrossed = false;
            lapThresholdCrossed = false;

            //Debug.Log("InitLocalRaceData  localRaceInfo=" + localRaceInfo.ToString());
        }

        void CountdownTimerFinished()
        {
            StartRace();
        }

        IEnumerator InvokeInit()
        {
            GameObject airGatesGO = null;

            while (airGatesGO == null)
            {
                airGatesGO = GameObject.Find(Constants.AirGatesName);
                yield return null;
            }

            airGates.Clear();

            foreach (Transform child in airGatesGO.transform)
            {
                //Debug.Log("Adding air gate " + child.gameObject.name);

                airGates.Add(child.gameObject.GetComponent<AirGate>());

                yield return null;
            }

            SetAirGateNumbers();

            yield return null;

            SetAllAirGatesNone();

            droneRacer = null;

            while (droneRacer == null)
            {
                droneRacer = Controller.DroneRacer.FindDroneRacer();
                yield return null;
            }

            droneRacer.RaceStatus = RaceStatus.None;

            droneRacerArm = droneRacer.GetComponent<DroneRacerArm>();

            InitLocalRaceData();

            raceManagerIsReady = true;

            initializing = false;
        }

        void SetAllAirGatesNone()
        {
            foreach (AirGate ag in airGates)
            {
                ag.SetState(AirGate.State.None);
            }
        }

        void SetAllAirGatesNotCrossed()
        {
            foreach (AirGate ag in airGates)
            {
                ag.SetState(AirGate.State.NotCrossed);
            }
        }

        void SetAirGateNext(int airGateIdx)
        {
            GetAirGate(airGateIdx).SetState(AirGate.State.Next);
        }

        void SetAirGateCrossed(int airGateIdx)
        {
            GetAirGate(airGateIdx).SetState(AirGate.State.Crossed);
        }

        AirGate GetAirGate(int idx)
        {
            return airGates[idx % airGates.Count];
        }

        void SetAirGateNumbers()
        {
            for (int i = 0; i < airGates.Count; i++)
            {
                string numberStr = (i + 1).ToString();

                Component c = SceneHelper.FindFirstChildInHierarchy(airGates[i], Constants.AirGateNumberFront);

                Text text;

                if (c != null)
                {
                    text = c.GetComponent<Text>();

                    if (text != null)
                    {
                        text.text = numberStr;
                    }
                }

                c = SceneHelper.FindFirstChildInHierarchy(airGates[i], Constants.AirGateNumberBack);

                if (c != null)
                {
                    text = c.GetComponent<Text>();

                    if (text != null)
                    {
                        text.text = numberStr;
                    }
                }
            }
        }

        int FindAirGate(AirGate airGate)
        {
            int retVal = -1;

            for (int i = 0; i < airGates.Count; i++)
            {
                if (airGate == airGates[i])
                {
                    retVal = i;
                    break;
                }
            }

            return retVal;
        }

        bool firstThresholdCrossed;
        bool lapThresholdCrossed;

        void HandleThreshold(AirGate airGate)
        {
            int airGateIdx = FindAirGate(airGate);

            if (airGateIdx < 0)
            {
                Debug.LogError("Air gate not found!");
                return;
            }

            if (firstThresholdCrossed == false && airGateIdx != 0)
            {
                return;
            }

            firstThresholdCrossed = true;

            if (airGateIdx == 0)
            {
                if (lapThresholdCrossed == true)
                {
                    lapCount++;

                    //Debug.Log(string.Format("lapCount={0}  totalLaps={1}", lapCount, raceInfo.totalLaps));

                    lapThresholdCrossed = false;

                    if (lapCount < raceInfo.totalLaps)
                    {
                        stopwatch.Lap();

                        UpdateRaceStatus(RaceStatus.InProgress);

                        SetAllAirGatesNotCrossed();

                    }
                    else
                    {
                        UpdateRaceStatus(RaceStatus.Finished);

                        SetAllAirGatesNone();

                        StopRace();
                    }
                }
                else
                {
                    lapThresholdCrossed = true;
                }
            }

            if (droneRacer.RaceStatus == RaceStatus.InProgress)
            {
                airGateIdx = airGateIdx + (lapCount * airGates.Count);

                SetAirGateCrossed(airGateIdx);

                if (airGateIdx > curAirGateIdx)
                {

                    curAirGateIdx = airGateIdx;

                    UpdateRaceStatus(RaceStatus.InProgress);

                    //Debug.Log("*** Passed thru air gate " + curAirGateIdx);
                }

                SetAirGateNext(curAirGateIdx + 1);
            }
        }

        void UpdateRaceStatus(RaceStatus raceStatus)
        {
            droneRacer.RaceStatus = raceStatus;
        }

        public static void StartRace()
        {
            if (IsReady() == true)
            {
                //Debug.Log("StartRace");

                instance.droneRacerArm.Unpause();

                instance.droneRacerArm.Disarm(true, false);

                instance.droneRacer.RaceStatus = RaceStatus.InProgress;

                instance.SetAllAirGatesNotCrossed();

                instance.SetAirGateNext(0);

                instance.stopwatch.StartStopwatch();

                instance.UpdateRaceStatus(RaceStatus.InProgress);

                instance.StartCoroutine(instance.WatchRaceStatus());
            }
        }

        public static void StopRace()
        {
            if (IsReady() == true)
            {
                //Debug.Log("StopRace()");

                instance.stopwatch.Stop();

                if (instance.droneRacer != null)
                {
                    instance.droneRacer.RaceStatus = RaceStatus.None;
                }

                instance.SetAllAirGatesNone();
            }
        }

        IEnumerator WatchRaceStatus()
        {
            while (IsRaceInProgress() == true)
            {
                yield return new WaitForSeconds(1);
            }

            //Debug.Log("Race is over.");
        }

        public static void AirGateThresholdCrossed(Controller.DroneRacer player, AirGate airGate)
        {
            if (IsRaceInProgress() == true && instance != null && instance.droneRacer == player)
            {
                //Debug.Log(string.Format("Air Gate {0} threshold crossed.", airGate.name));
                instance.HandleThreshold(airGate);
            }
        }

        public static bool IsReady()
        {
            bool retVal = false;

            if (instance != null)
            {
                retVal = instance.droneRacer != null && instance.raceManagerIsReady;
            }

            return retVal;
        }

        public static void GetReadyToRace()
        {
            if (IsRaceInProgress() == false)
            {
                if (instance != null)
                {
                    instance.droneRacer.RaceStatus = RaceStatus.GetReady;
                    instance.InitLocalRaceData();
                    instance.droneRacer.Restart(false, RestartAt.RestartAtStartingPos);
                    instance.droneRacerArm.Pause();
                    instance.countdownTimer.StartCountdown(instance.raceInfo.countdownSec, 0, false);
                }
            }
        }

        public static bool IsGetReady()
        {
            bool retVal = false;

            if (IsReady() == true)
            {
                DroneRacer dr = DroneRacer.FindDroneRacer();

                if (dr != null && dr.RaceStatus == RaceStatus.GetReady)
                {
                    retVal = true;
                }
            }

            return retVal;
        }

        public static bool IsRaceInProgress()
        {
            bool retVal = false;

            if (IsReady() == true)
            {
                DroneRacer dr = DroneRacer.FindDroneRacer();

                if (dr != null && dr.RaceStatus == RaceStatus.InProgress)
                {
                    retVal = true;
                }
            }

            return retVal;
        }

        public static bool IsAirGatePresent()
        {
            bool retVal = false;

            if (GameObject.Find(Constants.AirGatesName) != null)
            {
                retVal = true;
            }


            return retVal;
        }

        public static int GetLapCount()
        {
            int retVal = 0;

            if (IsReady() == true)
            {
                retVal = instance.stopwatch.LapCnt;
            }

            return retVal;
        }

        public static int GetTotalLaps()
        {
            int retVal = 0;

            if (IsReady() == true)
            {
                retVal = instance.raceInfo.totalLaps;
            }

            return retVal;
        }

        public static float GetElapsedSec()
        {
            float retVal = 0;

            if (IsReady() == true)
            {
                retVal = instance.stopwatch.ElapsedSec;
            }

            return retVal;
        }

        public static float GetLapElapsedSec()
        {
            float retVal = 0;

            if (IsReady() == true)
            {
                retVal = instance.stopwatch.LapElapsedSec;
            }

            return retVal;
        }

        public static float GetCountdownTimerTimeLeftSec()
        {
            float retVal = 0;

            if (IsReady() == true)
            {
                retVal = instance.countdownTimer.TimeLeftSec;
            }

            return retVal;
        }
    }
}