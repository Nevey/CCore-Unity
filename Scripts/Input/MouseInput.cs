using System;
using UnityEngine;

namespace CCore.Input
{
    public class MouseInputArgs : KeyboardInputArgs
    {
        public Vector2 position { get; set; }
    }

    public class MouseInput : MonoBehaviourSingleton<MouseInput>
    {
        private KeyCode[] keyCodes;

        private MouseInputArgs inputArgs = new MouseInputArgs();

        public event EventHandler<MouseInputArgs> InputDownEvent;

        public event EventHandler<MouseInputArgs> InputHoldEvent;

        public event EventHandler<MouseInputArgs> InputUpEvent;

        private void Awake()
        {
            // TODO: Handle cases when both mouse and touch screen is supported
            // Disable this component when on a touch screen device
            if (UnityEngine.Input.touchSupported)
            {
                LogWarning("Disabling mouse input since we're on a touch screen device");

                enabled = false;

                return;
            }

            keyCodes = new KeyCode[7];

            keyCodes[0] = KeyCode.Mouse0;
            keyCodes[1] = KeyCode.Mouse1;
            keyCodes[2] = KeyCode.Mouse2;
            keyCodes[3] = KeyCode.Mouse3;
            keyCodes[4] = KeyCode.Mouse4;
            keyCodes[5] = KeyCode.Mouse5;
            keyCodes[6] = KeyCode.Mouse6;
        }

        private void Update()
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                KeyCode keyCode = keyCodes[i];

                if (UnityEngine.Input.GetKeyDown(keyCode))
                {
                    DispatchInputDownEvent(keyCode);
                }

                if (UnityEngine.Input.GetKey(keyCode))
                {
                    DispatchInputHoldEvent(keyCode);
                }

                if (UnityEngine.Input.GetKeyUp(keyCode))
                {
                    DispatchInputUpEvent(keyCode);
                }
            }
        }

        private void DispatchInputDownEvent(KeyCode keyCode)
        {
            if (InputDownEvent != null)
            {
                inputArgs.position = UnityEngine.Input.mousePosition;

                inputArgs.keyCode = keyCode;

                Log("Mouse Input Down Event {0}", inputArgs);

                InputDownEvent(this, inputArgs);
            }
        }

        private void DispatchInputHoldEvent(KeyCode keyCode)
        {
            if (InputHoldEvent != null)
            {
                inputArgs.position = UnityEngine.Input.mousePosition;

                inputArgs.keyCode = keyCode;

                Log("Mouse Input Hold Event {0}", inputArgs);

                InputHoldEvent(this, inputArgs);
            }
        }

        private void DispatchInputUpEvent(KeyCode keyCode)
        {
            if (InputUpEvent != null)
            {
                inputArgs.position = UnityEngine.Input.mousePosition;
                
                inputArgs.keyCode = keyCode;

                Log("Mouse Input Up Event {0}", inputArgs);

                InputUpEvent(this, inputArgs);
            }
        }
    }
}