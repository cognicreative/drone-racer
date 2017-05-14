namespace DroneRacerFpv.Utils
{
    public static class InputHelper
    {
        public static bool JoystickConnected()
        {
            bool connected = false;

            string[] joystickNames = UnityEngine.Input.GetJoystickNames();

            if (joystickNames != null && joystickNames.Length > 0)
            {
                for (int i = 0; i < joystickNames.Length; i++)
                {
                    if (joystickNames[i] != string.Empty)
                    {
                        connected = true;
                        break;
                    }
                }
            }

            return connected;
        }

        public static bool MouseActive(float threshold = 0.1f)
        {
            //If there is no joystick connected then only clicking will return true
            return MouseClicked() || (MouseMoved(threshold) && JoystickConnected() == true);
        }

        public static bool MouseClicked()
        {
            bool clicked = UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetMouseButtonDown(1) || UnityEngine.Input.GetMouseButtonDown(2);

            return clicked;
        }

        public static bool MouseMoved(float threshold = 0.1f)
        {
            bool moved = false;

            float mousex = GetInputAxis("Mouse X");

            if (mousex < -threshold || mousex > threshold)
            {
                moved = true;
            }

            if (moved == false)
            {
                float mousey = GetInputAxis("Mouse Y");

                if (mousey < -threshold || mousey > threshold)
                {
                    moved = true;
                }
            }


            return moved;
        }

        public static float GetInputAxis(string name)
        {
            //Debug.Log("InputHelper.GetInputAxis name="+name);

            return UnityEngine.Input.GetAxis(name);
        }

        public static float GetInputAxisRaw(string name)
        {
            return UnityEngine.Input.GetAxisRaw(name);
        }

        public static bool GetButton(string name)
        {
            return UnityEngine.Input.GetButton(name);
        }

        public static bool GetButtonDown(string name)
        {
            return UnityEngine.Input.GetButtonDown(name);
        }

        public static bool ButtonToggle(string name, bool button, out bool buttonPressed)
        {
            buttonPressed = UnityEngine.Input.GetButtonDown(name);

            if (buttonPressed == true)
            {
                if (button == true)
                {
                    button = false;
                }
                else
                {
                    button = true;
                }
            }

            return button;
        }
    }
}