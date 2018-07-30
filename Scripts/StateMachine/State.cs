using System;
using UnityEngine;

namespace CCore.StateMachines
{
    public class State
    {
        public event Action PreEnterEvent;

        public event Action EnterEvent;

        public event Action PostEnterEvent;

        public event Action PreExitEvent;

        public event Action ExitEvent;

        public event Action PostExitEvent;

        /// <summary>
        /// Called by the state machine when transitioning in to this state
        /// </summary>
        public virtual void Enter()
        {
            Debug.LogFormat("<b>{0}</b> : <b>Enter</b>", this.GetType().Name);

            if (PreEnterEvent != null)
            {
                PreEnterEvent();
            }

            if (EnterEvent != null)
            {
                EnterEvent();
            }

            if (PostEnterEvent != null)
            {
                PostEnterEvent();
            }
        }

        /// <summary>
        /// Called by the state machine when transitioning away from this state
        /// </summary>
        public virtual void Exit()
        {
            Debug.LogFormat("<b>{0}</b> : <b>Exit</b>", this.GetType().Name);

            if (PreExitEvent != null)
            {
                PreExitEvent();
            }

            if (ExitEvent != null)
            {
                ExitEvent();
            }

            if (PostExitEvent != null)
            {
                PostExitEvent();
            }
        }
    }
}
