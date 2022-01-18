using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public event playerDeathDelegate OnPlayerDeath;

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
        // add our delegates to the event for the newly spawned player
        _player.GetComponent<Player>().OnPlayerDeath += OnPlayerDeath;
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
        OnPlayerDeath += HandlePlayerDeath;
    }

    private void Start()
    {
        _settings = GameSettings.Load();
    }

    private void Update() {
        if (_settings.Debug.ShowRespawn) {
            Debug.DrawLine(_player.transform.position, _spawnPoint);
        }
    }

    private void HandlePlayerDeath(PlayerDeathType type) {
        Respawn();
    }
}
