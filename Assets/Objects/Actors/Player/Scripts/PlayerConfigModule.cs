using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig", order = 1)]
public class PlayerConfigModule : ScriptableObject
{
    [SerializeField]
    // whether to render debug objects
    private bool _debug = false;

    // the force applied when the player moves, if the marble is already rolling
    [SerializeField]
    private float _continuousForce = 10;

    // extra gravity multipler applied when the player is moving down a slope
    [SerializeField]
    private float _slopeSlideBoost = 0;

    // extra impulse applied when the player is climbing a slope
    [SerializeField]
    private float _slopeClimbAssist = 0;

    [SerializeField]
    // max distance to cast ground detection spheres (must be > 0)
    private float _groundDetectionDistance = 0.1F;

    [SerializeField]
    // the radius of the sphers to cast for ground detection
    private float _groundDetectionRadius = 1;

    // extra force applied when the player reverses either the north/south or east/west axis
    [SerializeField]
    private float _reverseBoost = 0;

    // minimum slope before terrain is considered "not flat"
    [SerializeField]
    private float _slopeThreshold = 0;

    // the extra force applied when the player moves, if the marble is below a certain speed threshold
    // note this is buggy right now, and kicks in oddly on reversals
    [SerializeField]
    private float _stationaryBoost = 0;

    // if the player's speed is at or below this value, the 'initial force' will be applied on their next move
    [SerializeField]
    private float _stationaryThreshold = 0;

    // extra force applied when the player changes direction; ignored if _reverseBoost > 0
    [SerializeField]
    private float _turnBoost = 0;

    public float ContinuousForce {
        get { return _continuousForce; }
    }

    public float SlopeClimbAssist {
        get { return _slopeClimbAssist; }
    }

    public float SlopeSlideBoost {
        get { return _slopeSlideBoost; }
    }

    public bool Debug {
        get { return _debug; }
        set { _debug = value; }
    }

    public float GroundDetectionDistance {
        get { return _groundDetectionDistance; }
    }

    public float GroundDetectionRadius {
        get { return _groundDetectionRadius; }
    }

    public float ReverseBoost {
        get { return _reverseBoost; }
    }

    public float SlopeThreshold {
        get { return _slopeThreshold; }
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
