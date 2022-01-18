using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IFallEventEmitter))]
public class PlayerStateController : MonoBehaviour, IFallListener
{
    [SerializeField]
    private PlayerConfigModule _playerConfig;

    [SerializeField]
    private PlayerState _state;

    private void Start() {
        GetComponent<IFallEventEmitter>().AddFallListener(this);
    }

    private void OnDestroy() {
        GetComponent<IFallEventEmitter>().RemoveFallListener(this);
    }

    public void OnFalling() {
        _state.Fall();
    }

    public void OnLanded(ITerrainData terrain) {
        _state.Land();
    }

    public void OnFellOffLevel() {
        _state.FellTooFar();
    }

    public void OnShattered() {
        _state.Shatter();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Goal")) {
            _state.GoalReached();
        }
    }
}
