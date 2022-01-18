using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathType {
    public string Name { get; set; }
}

public delegate void playerDeathDelegate(PlayerDeathType type);

// Main behavior script for the player
[RequireComponent(typeof(IFallEventEmitter))]
public class Player : MonoBehaviour, IFallListener {
[SerializeField]
    private PlayerState _state;

    public event playerDeathDelegate OnPlayerDeath;

    private void Start() {
        GetComponent<IFallEventEmitter>().AddFallListener(this);
    }

    private void OnDestroy() {
        GetComponent<IFallEventEmitter>().RemoveFallListener(this);
    }

    public void OnFalling() { }

    public void OnLanded(ITerrainData terrain) { }

    public void OnFellOffLevel() {
        OnPlayerDeath?.Invoke(new PlayerDeathType() {
            Name = "freefall"
        });
    }

    public void OnShattered() {
        OnPlayerDeath?.Invoke(new PlayerDeathType() {
            Name = "shattered"
        });
    }
}
