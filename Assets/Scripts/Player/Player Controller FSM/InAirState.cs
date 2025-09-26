using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : PlayerState
{
    public InAirState(PlayerControllerFSM controller) : base(controller, PlayerStateType.IN_AIR) { }

    public override void EnterState()
    {
        // Debug.Log("Entering IN_AIR state");
    }

    public override void FixedUpdateState()
    {
        // Permetti movimento anche in aria
        controller.HandleMovement();
    }

    public override PlayerStateType CheckTransitions()
    {
        // Controllo per doppio salto
        if (controller.JumpInput && controller.CanJump())
        {
            return PlayerStateType.JUMPING;
        }

        // Se torna a terra
        if (controller.GroundChecker.IsGrounded)
        {
            // Se c'è movimento, va in moving, altrimenti idle
            if (controller.HasMovementInput())
            {
                return PlayerStateType.MOVING;
            }
            else
            {
                return PlayerStateType.IDLE;
            }
        }

        return PlayerStateType.IN_AIR;
    }
}