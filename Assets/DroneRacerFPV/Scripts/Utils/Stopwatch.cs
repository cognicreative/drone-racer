using System;
using System.Collections;
using UnityEngine;

namespace DroneRacerFpv.Utils
{
    public class Stopwatch : MonoBehaviour
    {
        #region StopwatchLap
        public delegate void StopwatchLap(float elapsedSec, int cnt);

        public event StopwatchLap stopwatchLapListeners;

        void StopwatchLapHandler(float elapsedSec, int cnt)
        {
            if (stopwatchLapListeners != null)
                stopwatchLapListeners(elapsedSec, cnt);
        }

        public void AddStopwatchLapListener(StopwatchLap stopwatchLap)
        {
            stopwatchLapListeners += stopwatchLap;
        }

        public void RemoveAllStopwatchLapFinishedListeners()
        {
            if (stopwatchLapListeners != null)
            {
                foreach (Delegate d in stopwatchLapListeners.GetInvocationList())
                {
                    stopwatchLapListeners -= (StopwatchLap)d;
                }
            }
        }
        #endregion

        #region StopwatchUpdate
        public delegate void StopwatchUpdate(float elapsedSec);

        public event StopwatchUpdate stopwatchUpdateListeners;

        void StopwatchUpdateHandler(float elapsedSec)
        {
            if (stopwatchUpdateListeners != null)
                stopwatchUpdateListeners(elapsedSec);
        }

        public void AddStopwatchUpdateListener(StopwatchUpdate stopwatchUpdate)
        {
            stopwatchUpdateListeners += stopwatchUpdate;
        }

        public void RemoveAllStopwatchUpdateListeners()
        {
            if (stopwatchUpdateListeners != null)
            {
                foreach (Delegate d in stopwatchUpdateListeners.GetInvocationList())
                {
                    stopwatchUpdateListeners -= (StopwatchUpdate)d;
                }
            }
        }
        #endregion

        public float updateWaitSecs = 0.01f;

        public void StartStopwatch(float delayStartSec = 0, bool removeListenersOnFinish = true)
        {
            stop = false;
            StartCoroutine(HandleStopwatch(delayStartSec, removeListenersOnFinish));
        }

        public void Lap()
        {
            lapStartSec = Time.time;
            lapCnt++;
        }

        public bool Running
        {
            get
            {
                return stop != true;
            }
        }

        float elapsedSec;
        public float ElapsedSec
        {
            get
            {
                return elapsedSec;
            }
        }

        float lapElapsedSec;
        public float LapElapsedSec
        {
            get
            {
                return lapElapsedSec;
            }
        }

        int lapCnt = 1;
        public int LapCnt
        {
            get
            {
                return lapCnt;
            }
        }

        public void ResetStopwatch()
        {
            lapCnt = 1;
            elapsedSec = 0;
            lapElapsedSec = 0;

            StopwatchUpdateHandler(elapsedSec);
            StopwatchLapHandler(lapElapsedSec, lapCnt);
        }

        public void AddTime(float timeSec)
        {
            startSec -= timeSec;
        }

        bool stop = true;
        public void Stop()
        {
            //Debug.Log("Stopwatch.Stop()");

            stop = true;

            UpdateListeners();
        }

        void UpdateListeners()
        {
            float t = Time.time;

            elapsedSec = t - startSec;
            lapElapsedSec = t - lapStartSec;

            StopwatchUpdateHandler(elapsedSec);
            StopwatchLapHandler(lapElapsedSec, lapCnt);
        }

        float startSec;
        float lapStartSec;
        IEnumerator HandleStopwatch(float delayStartSec, bool removeListenersOnFinish)
        {
            ResetStopwatch();

            if (delayStartSec > 0)
            {
                yield return new WaitForSeconds(delayStartSec);
            }

            startSec = Time.time;

            lapStartSec = startSec;

            while (stop == false)
            {
                UpdateListeners();
                yield return new WaitForSeconds(updateWaitSecs);
            }

            if (removeListenersOnFinish == true)
            {
                RemoveAllStopwatchUpdateListeners();
                RemoveAllStopwatchLapFinishedListeners();
            }
        }
    }
}