using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private readonly Queue<Vector3> _positions = new Queue<Vector3>();

    // initial position for newly spawned players
    [SerializeField]
    private Vector3 _initialPosition;

    [SerializeField]
    private BooleanValue inRespawnZone;

    // the most recently spawned player object, used to track movement for respawn checkpoints
    private GameObject _player;

    // the prefab representing the player object
    [SerializeField]
    private GameObject _playerPrefab;

    // boolean indicating if the player's current state indicates a safe respawn point
    private bool _safeSpawnState = true;

    // trigger emitted when the player should respawn
    [SerializeField]
    private PlayerState _state;

    void Start()
    {
        _state.AddStateChangeListener(OnStateChange);
        _state.Respawn();
    }

    private void FixedUpdate() {
        if (!_player) {
            return;
        }

        if (_safeSpawnState && inRespawnZone.Value) {
            _positions.Enqueue(_player.transform.position);
        }

        if (_positions.Count > 10) {
            _initialPosition = _positions.Dequeue();
        }
    }

    private void OnDestroy() {
        _state.RemoveStateChangeListener(OnStateChange);
    }

    private void OnStateChange(PlayerStateType state)
    {
        switch (state) {
            case PlayerStateType.Respawning:
                OnRespawn();
                break;

            // unsafe states for respawn
            case PlayerStateType.Falling:
            case PlayerStateType.WinningWhileFalling:
                _safeSpawnState = false;
                break;

            // unsafe states for respawn; also, die
            case PlayerStateType.FallingToDeath:
            case PlayerStateType.Shattering:
                _safeSpawnState = false;

                // TODO: have a separate shattering or falling to death object
                _state.Respawn();
                break;

            default:
                _safeSpawnState = true;
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
