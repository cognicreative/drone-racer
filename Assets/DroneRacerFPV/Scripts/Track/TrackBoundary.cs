using DroneRacerFpv.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace DroneRacerFpv.Track
{
    //[RequireComponent(typeof(BoxCollider))]
    [ExecuteInEditMode]
    public class TrackBoundary : MonoBehaviour
    {
        public float width = 1000;
        public float depth = 1000;
        public float height = 1000;

        float lastWidth;
        float lastDepth;
        float lastHeight;

        GameObject top;
        GameObject bottom;
        GameObject front;
        GameObject back;
        GameObject left;
        GameObject right;

        void Start()
        {
            top = SceneHelper.FindFirstChildInHierarchy<Transform>(this, "Top").gameObject;
            bottom = SceneHelper.FindFirstChildInHierarchy<Transform>(this, "Bottom").gameObject;
            front = SceneHelper.FindFirstChildInHierarchy<Transform>(this, "Front").gameObject;
            back = SceneHelper.FindFirstChildInHierarchy<Transform>(this, "Back").gameObject;
            left = SceneHelper.FindFirstChildInHierarchy<Transform>(this, "Left").gameObject;
            right = SceneHelper.FindFirstChildInHierarchy<Transform>(this, "Right").gameObject;

            UpdateSides();
        }

        void Update()
        {
            if (width != lastWidth || depth != lastDepth || height != lastHeight)
            {
                UpdateSides();
            }
        }

        void UpdateSides()
        {
            lastWidth = width;
            lastDepth = depth;
            lastHeight = height;

            top.transform.localEulerAngles = Vector3.zero;
            top.transform.localPosition = new Vector3(0, height, 0);
            top.transform.localScale = new Vector3(width / 10, 0 , depth / 10);

            bottom.transform.localEulerAngles = Vector3.zero;
            bottom.transform.localPosition = new Vector3(0, 0, 0);
            bottom.transform.localScale = new Vector3(width / 10, 0, depth / 10);

            front.transform.localEulerAngles = new Vector3(90, 0, 0);
            front.transform.localPosition = new Vector3(0, height / 2, depth / 2);
            front.transform.localScale = new Vector3(width / 10, 0, height / 10);

            back.transform.localEulerAngles = new Vector3(90, 0, 0);
            back.transform.localPosition = new Vector3(0, height / 2, -depth / 2);
            back.transform.localScale = new Vector3(width / 10, 0, height / 10);

            left.transform.localEulerAngles = new Vector3(90, 90, 0);
            left.transform.localPosition = new Vector3(-width / 2, height / 2, 0);
            left.transform.localScale = new Vector3(depth / 10, 0, height / 10);

            right.transform.localEulerAngles = new Vector3(90, 90, 0);
            right.transform.localPosition = new Vector3(width / 2, height / 2, 0);
            right.transform.localScale = new Vector3(depth / 10, 0, height / 10);

        }


        //public GameObject reentryPoint;

        //public void OnTriggerExit(Collider other)
        //{
        //    other.gameObject.transform.position = reentryPoint.transform.position;
        //}
    }
}