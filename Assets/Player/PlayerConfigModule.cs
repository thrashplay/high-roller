using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig", order = 1)]
public class PlayerConfigModule : ScriptableObject
{
    // the force applied when the player moves, if the marble is already rolling
    [SerializeField]
    private float _continuousForce = 10;
    
    // multiplier to apply to the player's gravity when they are falling
    [SerializeField]
    private float _freefallGravityMultipler = 2;

    // extra force applied when the player reverses either the north/south or east/west axis
    [SerializeField]
    private float _reverseBoost = 0;

    // the extra force applied when the player moves, if the marble is below a certain speed threshold
    // note this is buggy right now, and kicks in oddly on reversals
    [SerializeField]
    private float _stationaryBoost = 0;

    // extra force applied when the player changes direction; ignored if _reverseBoost > 0
    [SerializeField]
    private float _turnBoost = 0;

    // if the player's speed is at or below this value, the 'initial force' will be applied on their next move
    [SerializeField]
    private float _stationaryThreshold = 0;

    public float ContinuousForce {
        get { return _continuousForce; }
    }

    public float FreefallGravityMultiplier {
        get { return _freefallGravityMultipler; }
    }

    public float ReverseBoost {
        get { return _reverseBoost; }
    }

    public float StationaryBoost {
        get { return _stationaryBoost; }
    }

    public float StationaryThreshold {
        get { return _stationaryThreshold; }
    }

    public float TurnBoost {
        get { return _turnBoost; }
    }
}
