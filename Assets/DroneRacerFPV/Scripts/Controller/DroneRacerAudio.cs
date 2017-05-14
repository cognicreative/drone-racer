using DroneRacerFpv.Utils;
using System.Collections;
using UnityEngine;

namespace DroneRacerFpv.Controller
{
    [RequireComponent(typeof(DroneRacer))]
    [RequireComponent(typeof(DroneRacerArm))]
    public class DroneRacerAudio : MonoBehaviour
    {
        AudioSource motorSound;

        DroneRacer droneRacer;

        DroneRacerArm droneRacerArm;

        void Awake()
        {
            droneRacer = GetComponent<DroneRacer>();
            droneRacerArm = GetComponent<DroneRacerArm>();
            motorSound = GetComponent<AudioSource>();
        }

        public float motorVolume = 1;
        public float minPitch = 1f;

        void FixedUpdate()
        {
            if (droneRacer.Crashed == true)
            {
                if (makingCrashSound == false)
                {
                    makingCrashSound = true;

                    StartCoroutine(InvokeCrashSound());
                }

                return;
            }

            if (droneRacerArm.Armed() == false)
            {
                motorSound.Stop();

                //Debug.Log("motorSound.Stop()");

                return;
            }

            makingCrashSound = false;

            float speed = droneRacer.MotorSpeed;

            if (MathHelper.ValueInDeadZone(speed, 0.1f) == false)
            {
                if (motorSound.isPlaying == false)
                {
                    motorSound.Play();
                    //Debug.Log("motorSound.Play()");
                }

                motorSound.pitch = speed + minPitch;

                motorSound.volume = speed * Mathf.Clamp(motorVolume, 0, 1);
            }
            else
            {
                motorSound.Stop();
            }
        }

        bool makingCrashSound;
        IEnumerator InvokeCrashSound()
        {
            //Debug.Log("InvokeCrashSound");

            float pitchFactor = droneRacer.MotorSpeed;
            float maxPitchFactor = pitchFactor * 2.5f;

            while (pitchFactor < maxPitchFactor)
            {
                motorSound.pitch = pitchFactor + minPitch;

                motorSound.volume = pitchFactor * Mathf.Clamp(motorVolume, 0, 1);

                pitchFactor += 0.1f;

                yield return null;
            }

            motorSound.Stop();
        }
    }
}