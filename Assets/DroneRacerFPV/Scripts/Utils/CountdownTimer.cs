using System;
using System.Collections;
using UnityEngine;

namespace DroneRacerFpv.Utils
{
    public class CountdownTimer : MonoBehaviour
    {
        #region TimerFinished
        public delegate void TimerFinished();

        public event TimerFinished timerFinishedListeners;

        void TimerFinishedHandler()
        {
            if (timerFinishedListeners != null)
                timerFinishedListeners();
        }

        public void AddTimerFinishedListener(TimerFinished timerFinished)
        {
            timerFinishedListeners += timerFinished;
        }

        public void RemoveAllTimerFinishedListeners()
        {
            if (timerFinishedListeners != null)
            {
                foreach (Delegate d in timerFinishedListeners.GetInvocationList())
                {
                    timerFinishedListeners -= (TimerFinished)d;
                }
            }
        }
        #endregion

        #region TimerUpdate
        float timeLeftSec;
        public float TimeLeftSec
        {
            get
            {
                return timeLeftSec;
            }
        }

        public delegate void TimerUpdate(float timeLeftSec);

        public event TimerUpdate timerUpdateListeners;

        void TimerUpdateHandler(float timeLeftSec)
        {
            this.timeLeftSec = timeLeftSec;

            if (timerUpdateListeners != null)
                timerUpdateListeners(timeLeftSec);
        }

        public void AddTimerUpdateListener(TimerUpdate timerUpdate)
        {
            timerUpdateListeners += timerUpdate;
        }

        public void RemoveAllTimerUpdateListeners()
        {
            if (timerUpdateListeners != null)
            {
                foreach (Delegate d in timerUpdateListeners.GetInvocationList())
                {
                    timerUpdateListeners -= (TimerUpdate)d;
                }
            }
        }
        #endregion

        bool stop = true;
        public void Stop()
        {
            stop = true;
        }

        public float updateWaitSecs = 0.1f;

        public void StartCountdown(float timeSec, float delayStartSec = 0, bool removeListenersOnFinish = true)
        {
            //Debug.Log(string.Format("StartCountdown timeSec={0}  delayStartSec={1}  removeListenersOnFinish={2}", timeSec, delayStartSec, removeListenersOnFinish));
            StartCoroutine(HandleCountdown(timeSec, delayStartSec, removeListenersOnFinish));
        }

        IEnumerator HandleCountdown(float timeSec, float delayStartSec, bool removeListenersOnFinish)
        {
            //Debug.Log("HandleCountdown");

            stop = false;

            if (delayStartSec > 0)
            {
                yield return new WaitForSeconds(delayStartSec);
            }

            float startTime = Time.time;

            while (stop == false && startTime + timeSec > Time.time)
            {
                TimerUpdateHandler(timeSec - (Time.time - startTime));

                yield return new WaitForSeconds(updateWaitSecs);
            }

            TimerFinishedHandler();

            if (removeListenersOnFinish == true)
            {
                RemoveAllTimerUpdateListeners();
                RemoveAllTimerFinishedListeners();
            }
        }
    }
}