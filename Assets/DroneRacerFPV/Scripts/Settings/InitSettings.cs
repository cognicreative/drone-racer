using UnityEngine;
using System;

namespace DroneRacerFpv.Settings
{
    public static class InitSettings
    {
        public static void Initialize()
        {
            //Debug.Log("InitSettings.Initialize");

            if (PlayerPrefs.HasKey(Constants.AirMode) == false)
            {
                PlayerPrefs.SetInt(Constants.AirMode, Convert.ToInt32(Constants.AirModeDefault));
            }

            if (PlayerPrefs.HasKey(Constants.CameraAngle) == false)
            {
                PlayerPrefs.SetFloat(Constants.CameraAngle, Constants.CameraAngleDefault);
            }

            if (PlayerPrefs.HasKey(Constants.CameraFov) == false)
            {
                PlayerPrefs.SetFloat(Constants.CameraFov, Constants.CameraFovDefault);
            }

            if (PlayerPrefs.HasKey(Constants.PowerOutput) == false)
            {
                PlayerPrefs.SetFloat(Constants.PowerOutput, Constants.PowerOutputDefault);
            }

            if (PlayerPrefs.HasKey(Constants.AutoLevelStrength) == false)
            {
                PlayerPrefs.SetFloat(Constants.AutoLevelStrength, Constants.AutoLevelStrengthDefault);
            }

            if (PlayerPrefs.HasKey(Constants.AutoLevel) == false)
            {
                PlayerPrefs.SetInt(Constants.AutoLevel, Convert.ToInt32(Constants.AutoLevelDefault));
            }

            if (PlayerPrefs.HasKey(Constants.AltitudeHoldStrength) == false)
            {
                PlayerPrefs.SetFloat(Constants.AltitudeHoldStrength, Constants.AltitudeHoldStrengthDefault);
            }

            if (PlayerPrefs.HasKey(Constants.AltitudeHold) == false)
            {
                PlayerPrefs.SetInt(Constants.AltitudeHold, Convert.ToInt32(Constants.AltitudeHoldDefault));
            }

            if (PlayerPrefs.HasKey(Constants.DetectCrash) == false)
            {
                PlayerPrefs.SetInt(Constants.DetectCrash, Convert.ToInt32(Constants.DetectCrashDefault));
            }

            if (PlayerPrefs.HasKey(Constants.CrashTolerance) == false)
            {
                PlayerPrefs.SetFloat(Constants.CrashTolerance, Constants.CrashToleranceDefault);
            }

            if (PlayerPrefs.HasKey(Constants.RestartAt) == false)
            {
                PlayerPrefs.SetInt(Constants.RestartAt, (int)Constants.RestartAtDefault);
            }

            if (PlayerPrefs.HasKey(Constants.AutoRestart) == false)
            {
                PlayerPrefs.SetInt(Constants.AutoRestart, Convert.ToInt32(Constants.AutoRestartDefault));
            }

            if (PlayerPrefs.HasKey(Constants.AutoRestartDelay) == false)
            {
                PlayerPrefs.SetFloat(Constants.AutoRestartDelay, Constants.AutoRestartDelayDefault);
            }

            if (PlayerPrefs.HasKey(Constants.RotationRate) == false)
            {
                PlayerPrefs.SetFloat(Constants.RotationRate, Constants.RotationRateDefault);
            }

            if (PlayerPrefs.HasKey(Constants.ExponentialRate) == false)
            {
                PlayerPrefs.SetInt(Constants.ExponentialRate, Convert.ToInt32(Constants.ExponentialRateDefault));
            }

            if (PlayerPrefs.HasKey(Constants.ExponentialRateFactor) == false)
            {
                PlayerPrefs.SetFloat(Constants.ExponentialRateFactor, Constants.ExponentialRateFactorDefault);
            }

            if (PlayerPrefs.HasKey(Constants.ThrottleChangeRate) == false)
            {
                PlayerPrefs.SetFloat(Constants.ThrottleChangeRate, Constants.ThrottleChangeRateDefault);
            }

            if (PlayerPrefs.HasKey(Constants.TotalLaps) == false)
            {
                PlayerPrefs.SetFloat(Constants.TotalLaps, Constants.TotalLapsDefault);
            }

            if (PlayerPrefs.HasKey(Constants.CountdownTimeSec) == false)
            {
                PlayerPrefs.SetFloat(Constants.CountdownTimeSec, Constants.CountdownTimeSecDefault);
            }

            if (PlayerPrefs.HasKey(Constants.VelocityIndicator) == false)
            {
                PlayerPrefs.SetInt(Constants.VelocityIndicator, (int)Constants.VelocityIndicatorDefault);
            }

        }
    }
}