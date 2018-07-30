using System;
using UnityEngine;

namespace CCore.Input
{
    public class TouchInputArgs : EventArgs
    {
        public Vector2 position { get; set; }
        public Vector2 dragVelocity { get; set; }
        public int fingerId { get; set; }
        public TouchPhase touchPhase { get; set; }
        public int tapCount { get; set; }
        public float holdTime { get; set; }
    }
    
    public class TouchInput : MonoBehaviourSingleton<TouchInput>
    {
        private TouchInputArgs inputArgs = new TouchInputArgs();

        public event EventHandler<TouchInputArgs> InputEvent;

        private void Awake()
        {
            // Disable this component if touch is not supported on current device
            if (!UnityEngine.Input.touchSupported)
            {
                LogWarning("Disabling touch input");

                enabled = false;

                return;
            }
        }

        private void Update()
        {
            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                UnityEngine.Touch touch = UnityEngine.Input.touches[i];

                DispatchInput(touch);
            }
        }

        private void DispatchInput(UnityEngine.Touch touch)
        {
            if (InputEvent != null)
            {
                inputArgs.position = touch.position;

                inputArgs.dragVelocity = touch.deltaPosition;

                inputArgs.fingerId = touch.fingerId;

                inputArgs.touchPhase = touch.phase;

                inputArgs.tapCount = touch.tapCount;

                inputArgs.holdTime = touch.deltaTime;

                Log("Touch Input Event {0}", inputArgs);

                InputEvent(this, inputArgs);
            }
        }
    }
}