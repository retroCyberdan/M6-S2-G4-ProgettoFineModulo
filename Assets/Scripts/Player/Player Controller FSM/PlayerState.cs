using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    IDLE,
    MOVING,
    JUMPING,
    IN_AIR
}

public abstract class PlayerState
{
    protected PlayerControllerFSM controller;
    protected PlayerStateType stateType;

    public PlayerState(PlayerControllerFSM controller, PlayerStateType stateType)
    {
        this.controller = controller;
        this.stateType = stateType;
    }

    public PlayerStateType StateType => stateType;

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { }
    public virtual PlayerStateType CheckTransitions() { return stateType; }
}
