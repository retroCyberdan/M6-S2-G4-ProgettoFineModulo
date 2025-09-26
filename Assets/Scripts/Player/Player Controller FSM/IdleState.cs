using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerControllerFSM controller) : base(controller, PlayerStateType.IDLE) { }

    public override void EnterState()
    {
        // Debug.Log("Entering IDLE state");
    }

    public override void UpdateState()
    {
        // Logica per lo stato idle se necessaria
    }

    public override void FixedUpdateState()
    {
        // Non fare nulla quando fermo
    }

    public override PlayerStateType CheckTransitions()
    {
        // Reset contatore salti se a terra
        if (controller.GroundChecker.IsGrounded)
            controller.ResetJumpCount();

        // Priorit� al salto
        if (controller.JumpInput && controller.CanJump())
        {
            return PlayerStateType.JUMPING;
        }

        // Se non � a terra, � in aria
        if (!controller.GroundChecker.IsGrounded)
        {
            return PlayerStateType.IN_AIR;
        }

        // Se c'� movimento, passa a moving
        if (controller.HasMovementInput())
        {
            return PlayerStateType.MOVING;
        }

        return PlayerStateType.IDLE;
    }
}
