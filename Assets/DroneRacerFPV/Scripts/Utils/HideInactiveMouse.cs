using DroneRacerFpv.Input;
using System.Collections;
using UnityEngine;

namespace DroneRacerFpv.Utils
{
    public class HideInactiveMouse : MonoBehaviour
    {
        public float hideMouseDelaySec = 1;

        Coroutine invokeHideMouse;

        void Start()
        {
            HideMouse();
        }

        void Update()
        {
            if (InputHelper.MouseActive() == true)
            {
                ShowMouse();
            }
        }

        void ShowMouse()
        {
            if (invokeHideMouse != null)
            {
                StopCoroutine(invokeHideMouse);
                invokeHideMouse = null;
            }

            Cursor.visible = true;

            HideMouse();
        }

        void HideMouse()
        {
            invokeHideMouse = StartCoroutine(InovkeHideMouse());
        }

        IEnumerator InovkeHideMouse()
        {
            yield return new WaitForSeconds(hideMouseDelaySec);

            invokeHideMouse = null;

            Cursor.visible = false;
        }
    }
}