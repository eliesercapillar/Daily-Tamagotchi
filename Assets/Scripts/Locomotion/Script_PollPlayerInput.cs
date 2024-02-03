using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerLocomotion
{
    public class Script_PollPlayerInput : MonoBehaviour
    {
        // Movement Flags
        private bool isUpPressed, isLeftPressed, isDownPressed, isRightPressed;

        // Getters / Setters
        public bool IsUpPressed { get { return isUpPressed; } }
        public bool IsLeftPressed { get { return isLeftPressed; } }
        public bool IsDownPressed { get { return isDownPressed; } }
        public bool IsRightPressed { get { return isRightPressed; } }

        void Update()
        {
            if (Input.GetKey(KeyCode.W)) isUpPressed = true;
            if (Input.GetKey(KeyCode.A)) isLeftPressed = true;
            if (Input.GetKey(KeyCode.S)) isDownPressed = true;
            if (Input.GetKey(KeyCode.D)) isRightPressed = true;
        }
    }
}
