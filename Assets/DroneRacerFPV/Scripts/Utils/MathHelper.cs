using UnityEngine;

namespace DroneRacerFpv.Utils
{
    public static class MathHelper
    {
        public static float Sign(float f)
        {
            float retVal = 0;

            if (f != 0)
            {
                retVal = (f < 0f) ? -1f : 1f;
            }

            return retVal;
        }

        public static bool ValueInDeadZone(float value, float deadZone)
        {
            return Mathf.Abs(value) <= deadZone;
        }

        public static float DeadZoneClamp(float value, float deadZone, float clampValue = 0)
        {
            float retVal = value;

            if (ValueInDeadZone(value, deadZone) == true)
            {
                retVal = clampValue;
            }

            return retVal;
        }

        public static float ValueChangeInDeadZone(float value, float oldValue, float deadZone)
        {
            float retVal = value;

            if (ValueInDeadZone(value - oldValue, deadZone) == true)
            {
                retVal = oldValue;
            }

            return retVal;
        }
        public static float ScaleToRangeInverted(float fromValue, float fromMin, float fromMax, float toMin = 0, float toMax = 1, bool clampToRange = true, bool exponential = false, float expoFactor = 0)
        {
            float retVal = ScaleToRange(fromValue, fromMin, fromMax, toMin, toMax, clampToRange, exponential, expoFactor);

            //Invert the value within the "to" range, e.g. flip the value across the middle of the range.
            retVal = toMin - retVal + toMax;

            return retVal;
        }

        public static float ScaleToRange(float fromValue, float fromMin, float fromMax, float toMin = 0, float toMax = 1, bool clampToRange = true, bool exponential = false, float expoFactor = 0)
        {
            /*
                Although called exponential, most transmitters use a cubic equation to provide that 
                functionality as well as most racing games for "steering sensitivity". 
                For input and output ranges that go from -1 to +1, the equation is: 

                output = ( (1 - factor) x input3 ) + ( factor x input ) 
                
                factor = 1 is linear. 0 < factor < 1 is less sensitive in the center, 
                more sensitive at the ends of throw. 1 < factor <= 1.5 is more sensitive in the center,
                less sensitive at the outside. (factor > 1.5 results in peak output exceeding 1 at near peak inputs). 
                Some racing games adjust factor so that 50% is linear, < 50% is less sensitive in the middle (over 50% would not normally be used).

                Reference https://www.physicsforums.com/threads/equation-required-to-calculate-exponential-rate.524002/
            */

            float scalar;

            float scaledDiff;

            float retVal;

            if (exponential == true)
            {
                //Scale to -1 to 1 range to calculate exponential
                scalar = 2 / (fromMax - fromMin);

                scaledDiff = (fromValue - fromMin) * scalar;

                float expoVal = -1 + scaledDiff;

                expoVal = ((1 - expoFactor) * Mathf.Pow(expoVal, 3)) + (expoFactor * expoVal);

                //Now scale to the requested range
                scalar = (toMax - toMin) / 2;

                scaledDiff = (expoVal - -1) * scalar;

                retVal = toMin + scaledDiff;
            }
            else
            {
                scalar = (toMax - toMin) / (fromMax - fromMin);

                scaledDiff = (fromValue - fromMin) * scalar;

                retVal = toMin + scaledDiff;
            }

            if (clampToRange == true)
            {
                retVal = Mathf.Clamp(retVal, toMin, toMax);
            }

            return retVal;
        }

        public static bool Valid(Vector3 v)
        {
            bool valid = true;

            if (float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z))
            {
                valid = false;
            }

            return valid;
        }

        public static float NormalizedAngleDegrees(float angle, float shift = 0)
        {
            return angle + Mathf.Ceil((-angle + shift) / 360) * 360;
        }


    }
}