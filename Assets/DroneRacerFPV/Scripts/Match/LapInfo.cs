using System;
using UnityEngine;

namespace DroneRacerFpv.Match
{
    [Serializable]
    public class LapInfo
    {
        public float startTime;
        public float finishTime;

        public LapInfo()
        {
        }

        public LapInfo(float startTime, float finishTime)
        {
            this.startTime = startTime;
            this.finishTime = finishTime;
        }

        public LapInfo(LapInfo li)
        {
            startTime = li.startTime;
            finishTime = li.finishTime;
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        public static LapInfo Deserialize(string riString)
        {
           return JsonUtility.FromJson<LapInfo>(riString);
        }

    }
}