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
    private BooleanValue inRespawnZone;

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
        inRespawnZone.Value = false;
        isInSnow.Value = false;

        State = PlayerStateType.Respawning;
        return true;
    }

    public bool RespawnComplete() {
        switch (State) {
            case PlayerStateType.Respawning:
                State = PlayerStateType.Alive;
                return true;
        }

        return false;
    }

    public bool Fall() {
        switch (State) {
            case PlayerStateType.Alive:
            case PlayerStateType.Dizzy:
                State = PlayerStateType.Falling;
                return true;

            case PlayerStateType.Winning:
                State = PlayerStateType.WinningWhileFalling;
                return true;
        }

        return false;
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

    public bool Land(bool shatter = false) {
        switch (State) {
            case PlayerStateType.Falling:
                State = shatter ? PlayerStateType.Shattering : PlayerStateType.Alive;
                return true;

            case PlayerStateType.WinningWhileFalling:
                State = shatter ? PlayerStateType.Shattering : PlayerStateType.Winning;
                return true;
        }

        return false;
    }

    private void EmitStateChange(PlayerStateType state) {
        _stateChangeCallbacks.ForEach((callback) => callback(state));
    }
}
