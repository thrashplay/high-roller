using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathType {
    public string Name { get; set; }
}

public delegate void playerDeathDelegate(PlayerDeathType type);

// Main behavior script for the player
[RequireComponent(typeof(FallController))]
public class Player : MonoBehaviour {
[SerializeField]
    private PlayerState _state;

    public event playerDeathDelegate OnPlayerDeath;

    private void Start() {
        var fallController = GetComponent<FallController>();
        fallController.OnFellOffLevel += HandleFellOffLevel;
        fallController.OnShattered += HandleShattered;
    }

    private void OnDestroy() {
        var fallController = GetComponent<FallController>();
        fallController.OnFellOffLevel -= HandleFellOffLevel;
        fallController.OnShattered -= HandleShattered;
    }

    public void HandleFellOffLevel() {
        OnPlayerDeath?.Invoke(new PlayerDeathType() {
            Name = "freefall"
        });
    }

    public void HandleShattered() {
        OnPlayerDeath?.Invoke(new PlayerDeathType() {
            Name = "shattered"
        });
    }
}
