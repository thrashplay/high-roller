using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelChangeListener {
    void OnLevelLoad(LevelData level);

    void OnLevelUnload(LevelData level);
}

public class LevelManager : MonoBehaviour
{
    // root game object to hold the map
    [SerializeField]
    private GameObject mapRoot;

    // currently loaded level
    private LevelData _currentLevel;

    // the currently loaded map
    private GameObject _currentMap;

    // listeners interested in level change events
    private readonly List<ILevelChangeListener> _listeners =  new List<ILevelChangeListener>();

    // respawn controller used to spawn player objects
    private RespawnController _respawnController;

    public void AddListener(ILevelChangeListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(ILevelChangeListener listener)
    {
        _listeners.Remove(listener);
    }

    public void LoadLevel(LevelData level) {
        EmitLevelUnload(_currentLevel);

        _respawnController.Despawn();

        if (_currentMap != null) {
            Destroy(_currentMap);
        }

        Debug.LogFormat("Loading level: {0}", level.Description);
        _currentLevel = level;
        _currentMap = Instantiate(level.Map, mapRoot.transform);
        _respawnController.Respawn(level.InitialSpawnPoint);

        EmitLevelLoad(_currentLevel);

        Debug.Log("Level loading complete.");
    }

    private void Awake() {
        ServiceLocator.Instance.Register(this);
    
    }

    private void Start()
    {
        _respawnController = GetComponent<RespawnController>();
    }

    private void OnDestroy() {
        ServiceLocator.Instance.Unregister(this);
    }

    private void EmitLevelUnload(LevelData level) {
        if (level != null) {
            _listeners.ForEach((listener) => listener.OnLevelUnload(level));
        }
    }

    private void EmitLevelLoad(LevelData level) {
        if (level != null) {
            _listeners.ForEach((listener) => listener.OnLevelLoad(level));
        }
    }
}
