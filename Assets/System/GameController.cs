using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfigModule _playerConfig;

    private void Start() {
        _playerConfig.Debug = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Quitting...");
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Tilde)) {
            _playerConfig.Debug = !_playerConfig.Debug;
        }
    }
}
