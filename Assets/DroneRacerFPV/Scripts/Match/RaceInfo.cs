using System;
using UnityEngine;

namespace DroneRacerFpv.Match
{
    [Serializable]
    public class RaceInfo
    {
        public int totalLaps = 3;

        public float countdownSec = 3;

        public bool detectCrash = false;
        public float crashTolerance = 50;

        public RestartAt restartAt = RestartAt.RestartAtStartingPos;

        public RaceInfo()
        {
        }

        public RaceInfo(int totalLaps, float countdownSec, bool detectCrash, float crashTolerance, RestartAt restartAt)
        {
            this.totalLaps = totalLaps;

            this.countdownSec = countdownSec;

            this.detectCrash = detectCrash;
            this.crashTolerance = crashTolerance;

            this.restartAt = restartAt;
        }

        public RaceInfo(RaceInfo ri)
        {
            SetRaceInfo(ri);
        }

        public void SetRaceInfo(RaceInfo ri)
        {
            totalLaps = ri.totalLaps;

            countdownSec = ri.countdownSec;

            detectCrash = ri.detectCrash;
            crashTolerance = ri.crashTolerance;

            restartAt = ri.restartAt;
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        public static RaceInfo Deserialize(string riString)
        {
           return JsonUtility.FromJson<RaceInfo>(riString);
        }

        public override string ToString()
        {
            return string.Format("totalLaps={0}  countdownSec={1}  restartAt={2}  detectCrash={3}  crashTolerance={4}",
                                  totalLaps,     countdownSec,     restartAt,     detectCrash,     crashTolerance);
        }
    }
}