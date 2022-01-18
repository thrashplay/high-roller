using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType {
    Alive,
    Dead,
    Freefall,
    Shattering,
    Spawning,
}

public delegate void StateChangeCallback(PlayerStateType state);

[CreateAssetMenu(fileName = "PlayerState", menuName = "ScriptableObjects/PlayerState", order = 1)]
public class PlayerState : ScriptableObject
{
    private PlayerStateType _state = PlayerStateType.Alive;

    private readonly List<StateChangeCallback> _stateChangeCallbacks =  new List<StateChangeCallback>();

    [SerializeField]
    private BooleanValue isInSnow;

    [SerializeField]
    private TerrainType defaultTerrain;

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
        State = PlayerStateType.Alive;
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

    public bool Dying {
        get {
            return _state == PlayerStateType.Freefall || _state == PlayerStateType.Shattering;
        }
    }

    public bool Dead {
        get {
            return _state == PlayerStateType.Dead || _state == PlayerStateType.Spawning;
        }
    }

    public TerrainType CurrentTerrain { get; set; }

    public bool Die() {
        State = PlayerStateType.Dead;
        return true;
    }

    public bool FellTooFar() {
        // if we are already shattering or dead, don't change our state
        if (State == PlayerStateType.Shattering || State == PlayerStateType.Dead) {
            return false;
        }

        State = PlayerStateType.Freefall;
        return true;
    }

    public bool Shatter() {
        switch (State) {
            case PlayerStateType.Alive:
            case PlayerStateType.Freefall:
                State = PlayerStateType.Shattering;
                return true;
        }

        return false;
    }

    public bool Spawn() {
        State = PlayerStateType.Spawning;
        return true;
    }

    public bool SpawnComplete() {
        State = PlayerStateType.Alive;
        return true;
    }

    private void EmitStateChange(PlayerStateType state) {
        _stateChangeCallbacks.ForEach((callback) => callback(state));
    }
}
