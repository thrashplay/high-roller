using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    // emitted when the player begins to fall
    [SerializeField]
    private Trigger _fallingTrigger;

    // if state == Falling, the height at which the fall started. undefined otherwise
    private float _initialHeight;

    // emitted when the player lands
    [SerializeField]
    private TriggerWithTerrainData _landingTrigger;

    [SerializeField]
    private PlayerConfigModule _playerConfig;

    [SerializeField]
    private PlayerState _state;

    private void Start() {
        _fallingTrigger.AddListener(Fall);
        _landingTrigger.AddListener(Land);    
    }

    private void OnDestroy() {
        _fallingTrigger.RemoveListener(Fall);
        _landingTrigger.RemoveListener(Land);
    }

    private void Fall() {
        _initialHeight = transform.position.y;
        _state.Fall();
    }

    private void Land(ITerrainData terrain) {
        _state.Land(!IsSafeFall(terrain, _initialHeight, transform.position.y));
    }

    private void FixedUpdate() {
        if (_state.State == PlayerStateType.Falling || _state.State == PlayerStateType.WinningWhileFalling) {
            var fallDistance = _initialHeight - transform.position.y;
            if (fallDistance >= _playerConfig.EndlessFallHeight) {
                _state.FellTooFar();
            } 
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Goal")) {
            _state.GoalReached();
        }
    }

    // if the player landed at 'currentHeight' from a height of 'initialHeight' on given terrain, determines if the
    // fall was safe or not
    private bool IsSafeFall(ITerrainData terrain, float initialHeight, float currentHeight) {
        var fallDistance = initialHeight - currentHeight;
        var wasShort = fallDistance <= _playerConfig.SafeFallDistance;

        return terrain.TerrainType.SafeToFall || wasShort;
    }
}