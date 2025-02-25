using System.Collections.Generic;
using UnityEngine;
using Utilities.StateMachine;

namespace Combat
{
    public class CombatPlayerStateMachine : GenericStateMachine<CombatPlayerController>
    {
        protected CombatPlayerState GetCurrentSate() => (CombatPlayerState)currentState;

        public CombatPlayerStateMachine(CombatPlayerController owner) : base(owner)
        {
            States = new Dictionary<State, IState>
            {
                { State.SHEATHED, new CombatPlayerWeaponSheathedState(owner)},
            };
        }

        public void MoveInputPerformed(Vector2 input)
        {
            GetCurrentSate().OnMovePerformed(input);
        }

        public void MoveInputCanceled(Vector2 input)
        {
            GetCurrentSate().OnMoveCanceled(input);
        }
    }
}

