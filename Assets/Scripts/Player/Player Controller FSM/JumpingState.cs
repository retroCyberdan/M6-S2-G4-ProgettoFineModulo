using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public JumpingState(PlayerControllerFSM controller) : base(controller, PlayerStateType.JUMPING) { }

    private bool _hasExecutedJump = false;

    public override void EnterState()
    {
        if (!_hasExecutedJump)
        {
            controller.PerformJump();
            _hasExecutedJump = true;
        }
    }

    public override void ExitState()
    {
        _hasExecutedJump = false; // <- reset quando esce dallo stato
    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement(); // <- permetti movimento anche mentre salta
    }

    public override PlayerStateType CheckTransitions()
    {
        if (controller.GroundChecker.IsGrounded)
        {
            if (controller.HasMovementInput()) return PlayerStateType.MOVING;

            else return PlayerStateType.IDLE;
        }

        return PlayerStateType.JUMPING; // <- se non è a terra, rimane in jumping (gestisce la caduta)
    }
}