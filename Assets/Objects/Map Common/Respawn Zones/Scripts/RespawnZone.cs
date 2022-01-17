using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    // tag that will trigger the goal; default: Player
    [SerializeField]
    protected string triggeringTag = "Player";

    // cached Player script for the player object
    private readonly MemoizedComponent<Transform> _transformMemo = new MemoizedComponent<Transform>();

    private PlayerManager _playerManager;

    private void Start() {
        _playerManager = ServiceLocator.Instance.GetService<PlayerManager>();
        if (_playerManager == null) {
            Debug.LogWarning("[RespawnZone] No PlayerManager service registered.");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(triggeringTag)) {
            OnPlayerInside(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag(triggeringTag)) {
            OnPlayerInside(other.gameObject);
        }
    }

    private void OnPlayerInside(GameObject player) {
        var position = _transformMemo.Get(player).position;
        if (_playerManager != null && position != null) {
            _playerManager.UpdateSpawnPoint(position);
        }
    }
}
