using UnityEngine;

namespace Utilities.StateMachine
{
    public interface IState
    {
        public MonoBehaviour Owner { get; set; }

        void OnStateEnter();
        void Update(float TimeDeltaTime);
        void OnStateExit();
    }
}