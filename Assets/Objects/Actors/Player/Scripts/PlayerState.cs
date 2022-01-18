using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType {
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

public delegate void StateChangeCallback(PlayerStateType state);

[CreateAssetMenu(fileName = "PlayerState", menuName = "ScriptableObjects/PlayerState", order = 1)]
public class PlayerState : ScriptableObject
{
    private PlayerStateType _state = PlayerStateType.NeverSpawned;

    private readonly List<StateChangeCallback> _stateChangeCallbacks =  new List<StateChangeCallback>();

    [SerializeField]
    private BooleanValue isInSnow;

    [SerializeField]
    private TerrainType defaultTerrain;

    [SerializeField]
    private TerrainType snow;

    private void Awake() 
    {
        MovementState = MovementState.OnFlatTerrain;
    }
    
    public void AddStateChangeListener(StateChangeCallback listener)
    {
        _stateChangeCallbacks.Add(listener);
    }

    public void RemoveStateChangeListener(StateChangeCallback listener)
    {
        _stateChangeCallbacks.Remove(listener);
    }

    public MovementState MovementState { get; set; }

    public void Reset() {
        State = PlayerStateType.NeverSpawned;
    }

    public PlayerStateType State {
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
            return _state == PlayerStateType.Falling || _state == PlayerStateType.WinningWhileFalling;
        }
    }

    public TerrainType CurrentTerrain { get; set; }

    // return true if the state indicates the player has reached the goal
    public bool Winning {
        get {
            return _state == PlayerStateType.Winning || _state == PlayerStateType.WinningWhileFalling;
        }
    }

    public bool Respawn() {
        isInSnow.Value = false;

        State = PlayerStateType.Respawning;

        return true;
    }

    public bool RespawnComplete() {
        State = PlayerStateType.Alive;
        return true;
    }

    public bool Fall() {
        switch (State) {
            case PlayerStateType.Falling:
            case PlayerStateType.FallingToDeath:
            case PlayerStateType.WinningWhileFalling:
                // already falling!
                return false;

            case PlayerStateType.Winning:
                State = PlayerStateType.WinningWhileFalling;
                return true;

            default:
                State = PlayerStateType.Falling;
                return true;
        }
    }

    public bool FellTooFar() {
        if (Falling) {
            State = PlayerStateType.FallingToDeath;
            return true;
        }

        return false;
    }

    public bool GoalReached() {
        switch (State) {
            case PlayerStateType.Alive:
            case PlayerStateType.Dizzy:
            case PlayerStateType.Respawning:
                State = PlayerStateType.Winning;
                return true;

            case PlayerStateType.Falling:
                State = PlayerStateType.WinningWhileFalling;
                return true;
        }

        return false;
    }

    public bool Land() {
        switch (State) {
            case PlayerStateType.Falling:
            case PlayerStateType.WinningWhileFalling:
                State = PlayerStateType.Alive;
                return true;
        }

        return false;
    }

    public bool Shatter() {
        switch (State) {
            case PlayerStateType.Falling:
            case PlayerStateType.WinningWhileFalling:
                State = PlayerStateType.Shattering;
                return true;
        }

        return false;
    }

    private void EmitStateChange(PlayerStateType state) {
        _stateChangeCallbacks.ForEach((callback) => callback(state));
    }
}
