using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private readonly Queue<Vector3> _positions = new Queue<Vector3>();

    // initial position for newly spawned players
    [SerializeField]
    private Vector3 _initialPosition;

    // the most recently spawned player object, used to track movement for respawn checkpoints
    private GameObject _player;

    // the prefab representing the player object
    [SerializeField]
    private GameObject _playerPrefab;

    // boolean indicating if the player's current state indicates a safe respawn point
    private bool _safeSpawnPoint = true;

    // trigger emitted when the player should respawn
    [SerializeField]
    private PlayerStateMachine _state;

    void Start()
    {
        _state.AddStateChangeListener(OnStateChange);
        _state.Respawn();
    }

    private void FixedUpdate() {
        if (!_player) {
            return;
        }

        if (_safeSpawnPoint) {
            _positions.Enqueue(_player.transform.position);
        }

        if (_positions.Count > 10) {
            _initialPosition = _positions.Dequeue();
        }
    }

    private void OnDestroy() {
        _state.RemoveStateChangeListener(OnStateChange);
    }

    private void OnStateChange(PlayerState state)
    {
        switch (state) {
            case PlayerState.Respawning:
                OnRespawn();
                break;

            // unsafe states for respawn
            case PlayerState.Falling:
            case PlayerState.WinningWhileFalling:
                _safeSpawnPoint = false;
                break;

            // unsafe states for respawn; also, die
            case PlayerState.FallingToDeath:
            case PlayerState.Shattering:
                _safeSpawnPoint = false;

                // TODO: have a separate shattering or falling to death object
                _state.Respawn();
                break;

            default:
                _safeSpawnPoint = true;
                break;
        }
    }

    private void OnRespawn() {
        Destroy(_player);
        _player = Instantiate(_playerPrefab, _initialPosition, Quaternion.identity);
        _positions.Clear();
        
        // TODO: animate the respawn; respawning is currently instantaneous
        _state.RespawnComplete();
    }
}
