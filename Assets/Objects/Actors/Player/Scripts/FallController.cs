using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundSensor), typeof(Rigidbody))]
public class FallController : MonoBehaviour, IFallEventEmitter
{
    [SerializeField]
    private PlayerSettings playerSettings;

    // if state == Falling, then the height at which the fall started. undefined otherwise
    private float _initialHeight;

    // cached components
    private GroundSensor _groundSensor;
    private Rigidbody _rigidbody;
    private Transform _transform;

    // set of registered listeners
    private readonly List<IFallListener> _listeners =  new List<IFallListener>();

    public void AddFallListener(IFallListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveFallListener(IFallListener listener)
    {
        _listeners.Remove(listener);
    }

    public bool Falling { get; private set; }
    public float DistanceFell { 
        get {
            return _initialHeight - _transform.position.y;
        }
    }

    private void Start() {
        _groundSensor = GetComponent<GroundSensor>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void FixedUpdate() {
        EmitEvents();
        ApplyCustomGravity();
    }

    // update 
    private void EmitEvents() {
        var nowFalling = !_groundSensor.IsOnGround;
        if (!Falling && nowFalling) {
            _initialHeight = _transform.position.y;
            EmitFalling();
        } else if (Falling && !nowFalling) {
            var terrain = _groundSensor.CurrentTerrain;
            HandleLanding(terrain);
        }

        Falling = nowFalling;
        HandleFallingOffLevel();
    }

    // emit "fell off level" event, if needed
    private void HandleFallingOffLevel() {
        if (Falling) {
            if (DistanceFell >= playerSettings.Falling.MaxFreefall) {
                EmitFellOffLevel();
            }
        }
    }

    // apply custom gravity when falling
    private void ApplyCustomGravity() {
        if (Falling) {
            _rigidbody.AddForce(playerSettings.Falling.Gravity, ForceMode.Acceleration); 
        }
    }

    // check whether the landing was safe or not, and emit appropriate event
    private void HandleLanding(ITerrainData terrain) {
        var wasShortDistance = DistanceFell <= playerSettings.Falling.SafeFallHeight;
        var safeLanding = terrain.TerrainType.SafeToFall || wasShortDistance;

        if (safeLanding) {
            EmitLanded(terrain);
        } else { 
            EmitShattered();
        }
    }

    private void EmitFalling()
    {
        _listeners.ForEach((listener) => listener.OnFalling());
    }

    private void EmitFellOffLevel()
    {
        _listeners.ForEach((listener) => listener.OnFellOffLevel());
    }

    private void EmitLanded(ITerrainData terrain)
    {
        _listeners.ForEach((listener) => listener.OnLanded(terrain));
    }

    private void EmitShattered()
    {
        _listeners.ForEach((listener) => listener.OnShattered());
    }
}
