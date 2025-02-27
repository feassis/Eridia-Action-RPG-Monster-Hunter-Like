using UnityEngine;

namespace Combat
{
    public class CombatPlayerWeaponSheathedState : CombatPlayerState
    {
        public CombatPlayerWeaponSheathedState(MonoBehaviour owner) : base(owner) { }

        private const string animBlendTree = "Sheathed";
        private const string animParamSpeed = "Speed";
        private const float animDamping = 0.05f;

        public override void OnStateEnter()
        {
            GetCombatPlayerController().PlayCharacterAnimation(animBlendTree);
        }
        public override void Update(float TimeDeltaTime)
        {
            Vector3 movement = CalculateMovement();
            movement *= GetCombatPlayerController().MovementSpeed;
            Move(movement, TimeDeltaTime);

            GetCombatPlayerController().SetAnimationParam(animParamSpeed, 
                movement.magnitude / (GetCombatPlayerController().MovementSpeed * 2), animDamping
                , TimeDeltaTime);

            if(movement == Vector3.zero)
            {
                return;
            }

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

