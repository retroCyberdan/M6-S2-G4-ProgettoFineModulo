using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : PlayerState
{
    public MovingState(PlayerControllerFSM controller) : base(controller, PlayerStateType.MOVING) { }

    public override void EnterState()
    {
        // il reset è automatico nel PlayerControllerFSM
    }

    public override void FixedUpdateState()
    {
        controller.HandleMovement();
    }

    public override PlayerStateType CheckTransitions()
    {
        if (controller.JumpInput && controller.CanJump()) return PlayerStateType.JUMPING; // <- priorità al salto

        if (!controller.HasMovementInput()) return PlayerStateType.IDLE; // <- se non c'è input di movimento, vai in idle

        return PlayerStateType.MOVING; // <- continua a muoversi
    }
}