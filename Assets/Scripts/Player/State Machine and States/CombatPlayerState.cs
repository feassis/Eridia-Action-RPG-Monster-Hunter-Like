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
        
        protected Vector2 moveInput;

        protected Camera mainCamera;

        public CombatPlayerState(MonoBehaviour owner)
        {
            Owner = owner;
            mainCamera = Camera.main;
        }

        public CombatPlayerController GetCombatPlayerController() => (CombatPlayerController)Owner;

        public abstract void OnMovePerformed(Vector2 input);

        public abstract void OnMoveCanceled(Vector2 input);

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            Vector3 finalMotion = motion + GetCombatPlayerController().ForceReciever.Movement;
            finalMotion *= deltaTime;

            GetCombatPlayerController().CharacterController.Move(finalMotion);
        }

        protected Vector3 CalculateMovement()
        {
            Vector3 movement = new Vector3();

            movement += mainCamera.transform.right * moveInput.x;
            movement += mainCamera.transform.forward * moveInput.y;

            movement.y = 0;

            return movement.normalized;
        }

        protected void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            CombatPlayerController combatPlayerController = GetCombatPlayerController();
            combatPlayerController.transform.rotation = Quaternion.Lerp(combatPlayerController.transform.rotation,
                Quaternion.LookRotation(movement), combatPlayerController.RotationDamping * deltaTime);
        }
    }
}

