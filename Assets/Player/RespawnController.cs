using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    // number of second remaining before the checkpoint is confirmed
    private float _checkpointDelay = 0;

    // number of seconds before snapshotting checkpoints
    [SerializeField]
    private float _checkpointInterval = 2;

    // next possible spawnpoint, which will be confirmed if the player survives long enough
    private Vector3 _checkpointPosition;

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

        _checkpointPosition = _initialPosition;
        _checkpointDelay = 0;
    }

    private void FixedUpdate() {
        if (!_player) {
            return;
        }

        _checkpointDelay += Time.fixedDeltaTime;

        if (_checkpointDelay >= _checkpointInterval) {
            _initialPosition = _checkpointPosition;
            _checkpointDelay = 0;
            _checkpointPosition = _player.transform.position;
        }
    }

    private void OnDestroy() {
        _respawnTrigger.RemoveListener(OnRespawn);
    }

    private void OnRespawn()
    {
        _player = Instantiate(_playerPrefab, _initialPosition, Quaternion.identity);
    }
}
