using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public JumpingState(PlayerControllerFSM controller) : base(controller, PlayerStateType.JUMPING) { }

    public override void EnterState()
    {
        // Debug.Log("Entering JUMPING state");
        controller.PerformJump();
    }

    public override void FixedUpdateState()
    {
        // Permetti movimento anche durante il salto
        controller.HandleMovement();
    }

    public override PlayerStateType CheckTransitions()
    {
        // Dopo il frame di salto, vai in aria
        return PlayerStateType.IN_AIR;
    }
}