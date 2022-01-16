using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // root game object to hold the map
    [SerializeField]
    private GameObject mapRoot;

    [SerializeField]
    private PlayerConfigModule playerConfig;

    // respawn controller used to spawn player objects
    private RespawnController _respawnController;

    // the currently loaded map
    private GameObject _currentMap;

    public void LoadLevel(LevelData level) {
        _respawnController.Despawn();

        if (_currentMap != null) {
            Destroy(_currentMap);
        }

        Debug.LogFormat("Loading level: {0}", level.Description);
        _currentMap = Instantiate(level.Map, mapRoot.transform);
        _respawnController.Respawn(level.InitialSpawnPoint);
        Debug.Log("Level loading complete.");
    }

    private void Start() {
        playerConfig.Debug = false;
        _respawnController = GetComponent<RespawnController>();

        LoadLevel(ResourceManager.GetInstance().Load<LevelData>("level_select"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LoadLevel(ResourceManager.GetInstance().Load<LevelData>("level_select"));
        }

        if (Input.GetKeyDown(KeyCode.Tilde)) {
            playerConfig.Debug = !playerConfig.Debug;
        }
    }
}
