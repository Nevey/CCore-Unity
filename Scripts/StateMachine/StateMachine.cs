using System;
using System.Collections.Generic;

namespace CCore.StateMachines
{
    public class StateTransition
    {
        public State FromState { get; private set; }
        public State ToState { get; private set; }
        public Transition Transition { get; private set; }

        public StateTransition(State fromState, State toState, Transition transition)
        {
            FromState = fromState;
            ToState = toState;
            Transition = transition;
        }
    }
    
    public class StateMachine
    {
        private List<StateTransition> stateTransitions = new List<StateTransition>();

        protected StateMachine()
        {
            CurrentState = null;
        }

        public State CurrentState { get; private set; }

        private T GetTransition<T>() where T : Transition
        {
            Transition transition = null;

            for (int i = 0; i < stateTransitions.Count; i++)
            {
                StateTransition stateTransition = stateTransitions[i];

                if (typeof(T) == stateTransition.Transition.GetType())
                {
                    transition = stateTransition.Transition;
                }
            }

            return transition as T;
        }

        private StateTransition GetTransitionFromCurrentState<T>() where T : Transition
        {
            for (int i = 0; i < stateTransitions.Count; i++)
            {
                StateTransition stateTransition = stateTransitions[i];

                if (stateTransition.FromState.GetType() == CurrentState.GetType() &&
                    stateTransition.Transition.GetType() == typeof(T))
                {
                    return stateTransition;
                }
            }
            
            throw new Exception(String.Format(
                "Unable to find Transition <b><{0}></b> from State <b><{1}></b>",
                typeof(T).Name, CurrentState.GetType().Name));
        }

        protected void AddTransition<TFromState, TToState, TTransition>()
            where TFromState : State, new()
            where TToState : State, new()
            where TTransition : Transition, new()
        {
            // Create new states and new transition
            State newFromState = GetState<TFromState>() == null
                ? new TFromState()
                : GetState<TFromState>();

            State newToState = GetState<TToState>() == null 
                ? new TToState()
                : GetState<TToState>();

            Transition newTransition = GetTransition<TTransition>() == null
                ? new TTransition()
                : GetTransition<TTransition>();
            
            // Get their types
            Type newFromStateType = newFromState.GetType();
            Type newToStateType = newToState.GetType();
            Type newTransitionType = newTransition.GetType();

            // Check if this specific transition already exists
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (int i = 0; i < stateTransitions.Count; i++)
            {
                StateTransition stateTransition = stateTransitions[i];

                if (newFromStateType != stateTransition.FromState.GetType() || 
                    newToStateType != stateTransition.ToState.GetType() ||
                    newTransitionType != stateTransition.Transition.GetType())
                {
                    continue;
                }

                throw new Exception(String.Format(
                    "Transition <b><{0}></b> from State <b><{1}></b> to State <b><{2}></b> already exists!",
                    newTransitionType.Name, newFromStateType.Name, newToStateType.Name));
            }
            
            // Create new transition data...
            StateTransition newStateTransition =
                new StateTransition(newFromState, newToState, newTransition);
            
            // ...and add to the transition data list
            stateTransitions.Add(newStateTransition);
        }

        public void Start<T>() where T : State
        {
            CurrentState = GetState<T>();
            
            CurrentState.Enter();
        }

        public void Stop()
        {
            CurrentState.Exit();
            
            CurrentState = null;
        }

        public void DoTransition<T>() where T : Transition
        {
            StateTransition stateTransition = GetTransitionFromCurrentState<T>();
            
            CurrentState.Exit();
            
            CurrentState = stateTransition.ToState;
            
            CurrentState.Enter();
        }

        public T GetState<T>() where T : State
        {
            State state = null;
            
            for (int i = 0; i < stateTransitions.Count; i++)
            {
                StateTransition stateTransition = stateTransitions[i];

                if (typeof(T) == stateTransition.FromState.GetType())
                {
                    state = stateTransition.FromState;
                    
                    break;
                }

                // ReSharper disable once InvertIf
                if (typeof(T) == stateTransition.ToState.GetType())
                {
                    state = stateTransition.ToState;
                    
                    break;
                }
            }

            return state as T;
        }
    }
}
