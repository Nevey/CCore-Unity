using System;
using UnityEngine;

namespace CCore.Input
{
    public class KeyboardInputArgs : EventArgs
    {
        public KeyCode keyCode { get; set; }
    }

    public class KeyboardInput : MonoBehaviourSingleton<KeyboardInput>
    {
        private KeyCode[] keyCodes;

        private KeyboardInputArgs inputArgs = new KeyboardInputArgs();

        public event EventHandler<KeyboardInputArgs> InputDownEvent;

        public event EventHandler<KeyboardInputArgs> InputHoldEvent;

        public event EventHandler<KeyboardInputArgs> InputUpEvent;

        private void Awake()
        {
            keyCodes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];
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
                inputArgs.keyCode = keyCode;

                Log("Keyboard Input Down Event {0}", inputArgs);

                InputDownEvent(this, inputArgs);
            }
        }

        private void DispatchInputHoldEvent(KeyCode keyCode)
        {
            if (InputHoldEvent != null)
            {
                inputArgs.keyCode = keyCode;

                Log("Keyboard Input Hold Event {0}", inputArgs);

                InputHoldEvent(this, inputArgs);
            }
        }

        private void DispatchInputUpEvent(KeyCode keyCode)
        {
            if (InputUpEvent != null)
            {
                inputArgs.keyCode = keyCode;

                Log("Keyboard Input Up Event {0}", inputArgs);

                InputUpEvent(this, inputArgs);
            }
        }
    }
}