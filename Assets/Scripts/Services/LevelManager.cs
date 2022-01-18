using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // level displayed initially, and whenever the player reaches a goal
    [SerializeField]
    private LevelData defaultLevel;

    // root game object to hold the map
    [SerializeField]
    private GameObject mapRoot;

    // the currently loaded map
    private GameObject _currentMap;

    // respawn controller used to spawn player objects
    private PlayerManager _playerManager;

    // currently loaded level
    public LevelData CurrentLevel { get; private set; }

    public void LoadLevel(LevelData level) {
        _playerManager.Despawn();

        if (_currentMap != null) {
            Destroy(_currentMap);
        }

        Debug.LogFormat("Loading level: {0}", level.Description);
        CurrentLevel = level;
        _currentMap = Instantiate(level.Map, mapRoot.transform);
        _playerManager.Respawn(level.InitialSpawnPoint);
        Debug.Log("Level loading complete.");
    }

    private void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        LoadLevel(defaultLevel);
    }

    private void OnDestroy() {
        ServiceLocator.Instance.Unregister(this);
    }
}
