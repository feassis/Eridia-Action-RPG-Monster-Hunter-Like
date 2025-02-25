using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

namespace Utilities.StateMachine
{
    public class GenericStateMachine<T> where T : MonoBehaviour
    {
        protected T Owner;
        public IState currentState { get; protected set; }
        protected Dictionary<State, IState> States = new Dictionary<State, IState>();

        public GenericStateMachine(T Owner) => this.Owner = Owner;

        public void Update(float deltaTime) => currentState?.Update(deltaTime);

        protected void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void ChangeState(State newState) => ChangeState(States[newState]);

        protected void SetOwner()
        {
            foreach (IState state in States.Values)
            {
                state.Owner = Owner;
            }
        }
    }
}