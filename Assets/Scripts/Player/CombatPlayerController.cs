using Combat;
using System;
using UnityEngine;
using Utilities.StateMachine;

public class CombatPlayerController : MonoBehaviour
{
    protected PlayerInputControls inputControls;
    protected CombatPlayerStateMachine stateMachine;

    private void Awake()
    {
        stateMachine = new CombatPlayerStateMachine(this);

        inputControls = new PlayerInputControls();
        inputControls.Enable();

        inputControls.CombatMoveset.Move.performed += ctx => OnMovePerformed(ctx.ReadValue<Vector2>());
        inputControls.CombatMoveset.Move.canceled += ctx => OnMoveCanceled(ctx.ReadValue<Vector2>());
    }

    private void Start()
    {
        stateMachine.ChangeState(State.SHEATHED);
    }


    private void OnMoveCanceled(Vector2 vector2)
    {
        stateMachine.MoveInputCanceled(vector2);
    }

    private void OnMovePerformed(Vector2 vector2)
    {
        stateMachine.MoveInputPerformed(vector2);
    }

    private void OnEnable()
    {
        if(inputControls != null)
        {
            inputControls.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputControls != null)
        {
            inputControls.Disable();
        }
    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }
}
