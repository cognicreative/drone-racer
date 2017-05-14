using UnityEngine;
using System.Collections;
using System;
using DroneRacerFpv.Utils;

namespace DroneRacerFpv.Ui
{
    [RequireComponent(typeof(RectTransform))]
    public class JoystickThumbMover : MonoBehaviour
    {
        public RectTransform thumbArea;

        RectTransform rt;

        float halfWidth;
        float halfHeight;

        void Awake()
        {
            rt = GetComponent<RectTransform>();

            halfWidth = thumbArea.rect.width / 2;
            halfHeight = thumbArea.rect.height / 2;

            //Debug.Log(string.Format("JoystickThumbMover.Awake  halfWidth={0}  halfHeight={1}", halfWidth, halfHeight));
        }

        public void ResetPosition()
        {
            rt.localPosition = Vector3.zero;
        }

        public void LeftRight(float v, float minValue, float maxValue)
        {
            v = MathHelper.ScaleToRange(v, minValue, maxValue, -halfWidth, halfWidth);

            //Debug.Log(string.Format("JoystickThumbMover.LeftRight scaled v={0}  minValue={1}  maxValue{2}", v, minValue, maxValue));

            Vector3 pos = rt.localPosition;

            pos.x = v;

            rt.localPosition = pos;
        }

        public void UpDown(float v, float minValue, float maxValue)
        {
            v = MathHelper.ScaleToRange(v, minValue, maxValue, -halfHeight, halfHeight);

            //Debug.Log(string.Format("JoystickThumbMover.UpDown scaled v={0}  minValue={1}  maxValue{2}", v, minValue, maxValue));

            Vector3 pos = rt.localPosition;

            pos.y = v;

            rt.localPosition = pos;
        }
    }
}