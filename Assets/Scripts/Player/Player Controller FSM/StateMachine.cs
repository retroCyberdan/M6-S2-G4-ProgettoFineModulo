using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<PlayerStateType, PlayerState> _states;
    private PlayerState _currentState;
    private PlayerState _previousState;

    public StateMachine()
    {
        _states = new Dictionary<PlayerStateType, PlayerState>();
    }

    public void AddState(PlayerStateType stateType, PlayerState state)
    {
        _states[stateType] = state;
    }

    public void Initialize(PlayerStateType startingState)
    {
        if (_states.ContainsKey(startingState))
        {
            _currentState = _states[startingState];
            _currentState.EnterState();
        }
    }

    public void UpdateStateMachine()
    {
        if (_currentState == null) return;

        PlayerStateType nextStateType = _currentState.CheckTransitions(); // <- controlla transizioni

        if (nextStateType != _currentState.StateType)
        {
            ChangeState(nextStateType);
            return;
        }

        _currentState.UpdateState(); // <- aggiorna lo stato corrente
    }

    public void FixedUpdateStateMachine()
    {
        _currentState?.FixedUpdateState();
    }

    private void ChangeState(PlayerStateType newStateType)
    {
        if (!_states.ContainsKey(newStateType)) return;

        _currentState?.ExitState(); // <- exit dello stato corrente

        _previousState = _currentState;
        _currentState = _states[newStateType];

        _currentState.EnterState(); // <- enter del nuovo stato
    }

    public PlayerStateType GetCurrentStateType()
    {
        return _currentState?.StateType ?? PlayerStateType.IDLE;
    }

    public string GetStateInfo()
    {
        var current = _currentState?.StateType.ToString() ?? "None";
        var previous = _previousState?.StateType.ToString() ?? "None";
        return $"Current: {current}, Previous: {previous}";
    }
}