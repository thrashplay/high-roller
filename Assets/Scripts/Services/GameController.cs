using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // root game object to hold the map
    [SerializeField]
    private PlayerConfigModule playerConfig;

    private LevelManager _levelManager;

    public static GameController GetInstance() {
        return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start() {
        playerConfig.Debug = false;

        _levelManager = ServiceLocator.Instance.GetService<LevelManager>();
        _levelManager.LoadLevel(ResourceManager.GetInstance().Load<LevelData>("level_select"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _levelManager.LoadLevel(ResourceManager.GetInstance().Load<LevelData>("level_select"));
        }

        if (Input.GetKeyDown(KeyCode.Tilde)) {
            playerConfig.Debug = !playerConfig.Debug;
        }
    }
}
