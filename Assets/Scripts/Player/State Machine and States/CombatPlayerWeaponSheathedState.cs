using UnityEngine;

namespace Combat
{
    public class CombatPlayerWeaponSheathedState : CombatPlayerState
    {
        public CombatPlayerWeaponSheathedState(MonoBehaviour owner) : base(owner) { }

        public override void OnStateEnter()
        {

        }
        public override void Update(float TimeDeltaTime)
        {
            Vector3 movement= CalculateMovement();
            movement *= GetCombatPlayerController().MovementSpeed;
            Move(movement, TimeDeltaTime);
            FaceMovementDirection(movement, TimeDeltaTime);
        }
        public override void OnStateExit()
        {

        }


        public override void OnMovePerformed(Vector2 input)
        {
            moveInput = input;
        }
        public override void OnMoveCanceled(Vector2 input)
        {
            moveInput = Vector2.zero;
        }
    }
}

