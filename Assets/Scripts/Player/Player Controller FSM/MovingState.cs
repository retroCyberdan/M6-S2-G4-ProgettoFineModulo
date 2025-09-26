using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : PlayerState
{
    public MovingState(PlayerControllerFSM controller) : base(controller, PlayerStateType.MOVING) { }

    public override void EnterState()
    {
        // Debug.Log("Entering MOVING state");
    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement();
    }

    public override PlayerStateType CheckTransitions()
    {
        // Reset contatore salti se a terra
        if (controller.GroundChecker.IsGrounded)
            controller.ResetJumpCount();

        // Priorità al salto
        if (controller.JumpInput && controller.CanJump())
        {
            return PlayerStateType.JUMPING;
        }

        // Se non è a terra, è in aria
        if (!controller.GroundChecker.IsGrounded)
        {
            return PlayerStateType.IN_AIR;
        }

        // Se non c'è più movimento, torna idle
        if (!controller.HasMovementInput())
        {
            return PlayerStateType.IDLE;
        }

        return PlayerStateType.MOVING;
    }
}