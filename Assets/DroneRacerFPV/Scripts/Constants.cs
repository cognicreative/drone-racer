using DroneRacerFpv.Match;
using DroneRacerFpv.Settings;

namespace DroneRacerFpv
{
    public static class Constants
    {
        public const string StartPositionsName = "StartPositions";

        public const string AirGatesName = "AirGates";
        public const string AirGateNumberFront = "AirGateNumberFront";
        public const string AirGateNumberBack = "AirGateNumberBack";
        public const string AirGateLightName = "AirGateLight";

        public const string StartRace = "Start Race";
        public const string StopRace = "Stop Race";
        public const string GetReadyToRace = "Get Ready!";
        public const string LapLabelFormat = "Lap {0}:";
        public const string ElapsedLapTime = "00:00";

        public const string Armed = "Armed";
        public const string Disarmed = "Disarmed";

        public const float Scale = 10f;
        public const float InverseScale = 0.1f;

        //Length conversion
        public const float Meter2Kilometer = 0.001f;
        public const float Kilometer2Meter = 1000f;
        public const float Meter2Foot = 3.28084f;
        public const float Meter2Mile = 0.000621371f;
        public const float Kilometer2Mile = 0.621371f;

        //Speed conversion
        public const float MetersPerSec2FeetPerSec = 3.28084f;
        public const float MetersPerSec2KilometersPerHr = 3.6f;
        public const float MetersPerSec2MilesPerHr = 2.23694f;

        //GameObject names
        public const string FpvCameraName = "DroneRacerFpvCamera";

        //PlayerPrefs names and default values
        public const string CalibrationChecked = "CalibrationChecked";

        public const string AirMode = "AirMode";
        public const bool AirModeDefault = true;

        public const string CameraAngle = "CameraAngle";
        public const float CameraAngleDefault = 20;

        public const string CameraFov = "CameraFov";
        public const float CameraFovDefault = 50;

        public const string PowerOutput = "PowerOutput";
        public const float PowerOutputDefault = 5f;

        public const string AutoLevelStrength = "AutoLevelStrength";
        public const float AutoLevelStrengthDefault = 6;

        public const string AutoLevel = "AutoLevel";
        public const bool AutoLevelDefault = false;

        public const string AltitudeHoldStrength = "AltitudeHoldStrength";
        public const float AltitudeHoldStrengthDefault = 50;

        public const string AltitudeHold = "AltitudeHold";
        public const bool AltitudeHoldDefault = false;

        public const string DetectCrash = "DetectCrash";
        public const bool DetectCrashDefault = false;

        public const string CrashTolerance = "CrashTolerance";
        public const float CrashToleranceDefault = 50;

        public const string AutoRestart = "AutoRestart";
        public const bool AutoRestartDefault = true;

        public const string AutoRestartDelay = "AutoRestartDelay";
        public const float AutoRestartDelayDefault = 2;

        public const string RestartAt = "RestartAt";
        public const RestartAt RestartAtDefault = Match.RestartAt.RestartAtCrashPos;

        public const string RotationRate = "RotationRate";
        public const float RotationRateDefault = 1;

        public const string ExponentialRate = "ExponentialRate";
        public const bool ExponentialRateDefault = false;

        public const string ExponentialRateFactor = "ExponentialRateFactor";
        public const float ExponentialRateFactorDefault = 0.5f;

        public const string ThrottleChangeRate = "ThrottleChangeRate";
        public const float ThrottleChangeRateDefault = 2000f;

        public const string TotalLaps = "TotalLaps";
        public const int TotalLapsDefault = 3;

        public const string CountdownTimeSec = "CountdownTimeSec";
        public const float CountdownTimeSecDefault = 3;

        public const string VelocityIndicator = "VelocityIndicator";
        public const VelocityRate VelocityIndicatorDefault = VelocityRate.KilometersPerHour;

    }
}