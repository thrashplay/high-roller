using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private FallDetector _fallDetector;

    private readonly Queue<Vector3> _positions = new Queue<Vector3>();

    // initial position for newly spawned players
    [SerializeField]
    private Vector3 _initialPosition;

    // the most recently spawned player object, used to track movement for respawn checkpoints
    private GameObject _player;

    // the prefab representing the player object
    [SerializeField]
    private GameObject _playerPrefab;

    // trigger emitted when the player should respawn
    [SerializeField]
    private Trigger _respawnTrigger;

    void Start()
    {
        _respawnTrigger.AddListener(OnRespawn);
        OnRespawn();
    }

    private void FixedUpdate() {
        if (!_player || !_fallDetector) {
            return;
        }

        if (IsSafeRespawnPoint(_player.transform.position)) {
            _positions.Enqueue(_player.transform.position);
        }

        if (_positions.Count > 10) {
            _initialPosition = _positions.Dequeue();
        }
    }

    private bool IsSafeRespawnPoint(Vector3 position) {
        return !_fallDetector.Falling;
    }

    private void OnDestroy() {
        _respawnTrigger.RemoveListener(OnRespawn);
    }

    private void OnRespawn()
    {
        _player = Instantiate(_playerPrefab, _initialPosition, Quaternion.identity);
        _positions.Clear();
        _fallDetector = _player.GetComponent<FallDetector>();
    }
}
