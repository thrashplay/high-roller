using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody _body;

    private PlayerInputController _inputController;

    [SerializeField]
    private PlayerConfigModule _playerConfig;

    [SerializeField]
    // player state, used to determine if input is allowed or not
    private PlayerStateMachine _playerState;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _inputController = GetComponent<PlayerInputController>();
    }

    void FixedUpdate()
    {
        if (IsInputAllowed()) {
            var currentDirection = _inputController.GetRequestedDirection();

            if (currentDirection != Direction.None) {
                var position = new Vector3(0, 1, 0);
                var vector = _inputController.GetInputVector();

                // we apply only one of stationary boost, reverse boost or turn boost, favoring them in that order
                if (_playerConfig.StationaryBoost > 0 && !IsMoving()) {
                    _body.AddForceAtPosition(_playerConfig.StationaryBoost * vector, position, ForceMode.Impulse);
                } else if (_playerConfig.ReverseBoost > 0 && _inputController.IsReversed()) {
                    _body.AddForceAtPosition(_playerConfig.ReverseBoost * vector, position, ForceMode.Impulse);
                } else if (_playerConfig.TurnBoost > 0 && _inputController.IsTurned()) {
                    _body.AddForceAtPosition(_playerConfig.TurnBoost * vector, position, ForceMode.Impulse);
                }

                _body.AddForceAtPosition(_playerConfig.ContinuousForce * vector, position, ForceMode.Force);
            }
        }

        if (_playerState.Winning && !_playerState.Falling) {
            _body.angularVelocity *= 0.95F;
            _body.velocity *= 0.95F;
        }
    }

    private bool IsInputAllowed() {
        return _playerState.State switch {
            PlayerState.Winning => false,
            PlayerState.WinningWhileFalling => false,
            _ => true
        };
    }

    // uses the player config's "stationary threshold" to determine if the player is moving
    private bool IsMoving()
    {
        return _body.velocity.magnitude >= _playerConfig.StationaryThreshold;
    }
}
