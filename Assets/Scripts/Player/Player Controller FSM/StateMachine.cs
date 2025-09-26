using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<PlayerStateType, PlayerState> states;
    private PlayerState currentState;
    private PlayerState previousState;

    public StateMachine()
    {
        states = new Dictionary<PlayerStateType, PlayerState>();
    }

    public void AddState(PlayerStateType stateType, PlayerState state)
    {
        states[stateType] = state;
    }

    public void Initialize(PlayerStateType startingState)
    {
        if (states.ContainsKey(startingState))
        {
            currentState = states[startingState];
            currentState.EnterState();
        }
    }

    public void UpdateStateMachine()
    {
        if (currentState == null) return;

        // Controlla transizioni
        PlayerStateType nextStateType = currentState.CheckTransitions();

        if (nextStateType != currentState.StateType)
        {
            ChangeState(nextStateType);
        }

        // Aggiorna lo stato corrente
        currentState.UpdateState();
    }

    public void FixedUpdateStateMachine()
    {
        currentState?.FixedUpdateState();
    }

    private void ChangeState(PlayerStateType newStateType)
    {
        if (!states.ContainsKey(newStateType)) return;

        // Exit dello stato corrente
        currentState?.ExitState();

        previousState = currentState;
        currentState = states[newStateType];

        // Enter del nuovo stato
        currentState.EnterState();
    }

    public PlayerStateType GetCurrentStateType()
    {
        return currentState?.StateType ?? PlayerStateType.IDLE;
    }

    public string GetStateInfo()
    {
        var current = currentState?.StateType.ToString() ?? "None";
        var previous = previousState?.StateType.ToString() ?? "None";
        return $"Current: {current}, Previous: {previous}";
    }
}