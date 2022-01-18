using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // the prefab representing the player object
    [SerializeField]
    private GameObject _playerPrefab;
    
    // position for newly spawned players
    [SerializeField]
    private Vector3 _spawnPoint;

    // trigger emitted when the player should respawn
    [SerializeField]
    private PlayerState _state;

    // the most recently spawned player object, used to track movement for respawn checkpoints
    private GameObject _player;

    private GameSettings _settings;

    public void Despawn() {
        if (_player != null) {
            _player.SetActive(false);
            Destroy(_player);
            _player = null;
        }
    }

    // respawns the player at the last checkpoint
    public void Respawn() {
        Despawn();

        Debug.LogFormat("Respawning player at {0}.", _spawnPoint);
        _player = Instantiate(_playerPrefab, _spawnPoint, Quaternion.identity);

        // TODO: animate the respawn; respawning is currently instantaneous
        _state.RespawnComplete();
    }

    // respawns the player at the specified location, setting it as the latest checkpoint
    public void Respawn(Vector3 location) {
        UpdateSpawnPoint(location);
        Respawn();
    }

    // Updates the player's spawn point to the specified location
    public void UpdateSpawnPoint(Vector3 location) {
        _spawnPoint = location;
    }

    private void Awake() {
        ServiceLocator.Instance.Register(this);
    }

    private void Start()
    {
        _state.AddStateChangeListener(OnStateChange);
        _settings = GameSettings.Load();
    }

    private void Update() {
        if (_settings.Debug.ShowRespawn) {
            Debug.DrawLine(_player.transform.position, _spawnPoint);
        }
    }

    private void OnDestroy() {
        _state.RemoveStateChangeListener(OnStateChange);
    }

    private void OnStateChange(PlayerStateType state)
    {
        switch (state) {
            case PlayerStateType.Respawning:
                Respawn();
                break;

            // unsafe states for respawn
            case PlayerStateType.Falling:
            case PlayerStateType.WinningWhileFalling:
                // _safeSpawnState = false;
                break;

            // unsafe states for respawn; also, die
            case PlayerStateType.FallingToDeath:
            case PlayerStateType.Shattering:
                // _safeSpawnState = false;
                // TODO: have a separate shattering or falling to death object
                _state.Respawn();
                break;

            // default:
            //     _safeSpawnState = true;
            //     break;
        }
    }
}
