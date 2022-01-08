using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Alive,
    Dizzy,
    Falling,
    FallingToDeath,
    NeverSpawned,
    Respawning,
    Shattering,
    Winning,
    WinningWhileFalling
}

public delegate void StateChangeCallback(PlayerState state);

[CreateAssetMenu(fileName = "PlayerState", menuName = "ScriptableObjects/PlayerState", order = 1)]
public class PlayerStateMachine : ScriptableObject
{
    private PlayerState _state = PlayerState.NeverSpawned;

    private readonly List<StateChangeCallback> _stateChangeCallbacks =  new List<StateChangeCallback>();

    public void AddStateChangeListener(StateChangeCallback listener)
    {
        _stateChangeCallbacks.Add(listener);
    }

    public void RemoveStateChangeListener(StateChangeCallback listener)
    {
        _stateChangeCallbacks.Remove(listener);
    }

    public PlayerState State {
        get { return _state; }
        private set { 
            if (State != value) {
                _state = value; 
                EmitStateChange(_state);
            }
        }
    }

    // return true if the state indicates the player is in freefall
    public bool Falling {
        get {
            return _state == PlayerState.Falling || _state == PlayerState.WinningWhileFalling;
        }
    }

    // return true if the state indicates the player has reached the goal
    public bool Winning {
        get {
            return _state == PlayerState.Winning || _state == PlayerState.WinningWhileFalling;
        }
    }

    public bool Respawn() {
        State = PlayerState.Respawning;
        return true;
    }

    public bool RespawnComplete() {
        switch (State) {
            case PlayerState.Respawning:
                State = PlayerState.Alive;
                return true;
        }

        return false;
    }

    public bool Fall() {
        switch (State) {
            case PlayerState.Alive:
            case PlayerState.Dizzy:
                State = PlayerState.Falling;
                return true;

            case PlayerState.Winning:
                State = PlayerState.WinningWhileFalling;
                return true;
        }

        return false;
    }

    public bool FellTooFar() {
        if (Falling) {
            State = PlayerState.FallingToDeath;
            return true;
        }

        return false;
    }

    public bool GoalReached() {
        switch (State) {
            case PlayerState.Alive:
            case PlayerState.Dizzy:
            case PlayerState.Respawning:
                State = PlayerState.Winning;
                return true;

            case PlayerState.Falling:
                State = PlayerState.WinningWhileFalling;
                return true;
        }

        return false;
    }

    public bool Land(bool shatter = false) {
        switch (State) {
            case PlayerState.Falling:
                State = shatter ? PlayerState.Shattering : PlayerState.Alive;
                return true;

            case PlayerState.WinningWhileFalling:
                State = shatter ? PlayerState.Shattering : PlayerState.Winning;
                return true;
        }

        return false;
    }

    private void EmitStateChange(PlayerState state) {
        _stateChangeCallbacks.ForEach((callback) => callback(state));
    }
}
