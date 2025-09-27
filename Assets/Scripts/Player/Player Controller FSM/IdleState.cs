using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerControllerFSM controller) : base(controller, PlayerStateType.IDLE) { }

    public override void EnterState()
    {
        // il reset è automatico nel PlayerControllerFSM
    }

    public override void FixedUpdateState()
    {
        // niente movimento in idle
    }

    public override PlayerStateType CheckTransitions()
    {
        if (controller.JumpInput && controller.CanJump()) return PlayerStateType.JUMPING; // <- priorità al salto

        if (controller.HasMovementInput()) return PlayerStateType.MOVING; // <- poi al movimento

        return PlayerStateType.IDLE; // <- rimane in idle
    }
}