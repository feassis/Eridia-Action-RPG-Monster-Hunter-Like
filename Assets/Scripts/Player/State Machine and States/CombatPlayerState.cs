using UnityEngine;
using Utilities.StateMachine;

namespace Combat
{
    public abstract class CombatPlayerState : IState
    {
        public MonoBehaviour Owner { get; set; }
        public abstract void OnStateEnter();
        public abstract void Update(float TimeDeltaTime);
        public abstract void OnStateExit();

        public CombatPlayerState(MonoBehaviour owner)
        {
            Owner = owner;
        }

        public abstract void OnMovePerformed(Vector2 input);

        public abstract void OnMoveCanceled(Vector2 input);
    }
}

