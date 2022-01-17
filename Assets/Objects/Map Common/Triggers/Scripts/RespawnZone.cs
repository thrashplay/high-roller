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

    private RespawnController _respawnController;

    private void Start() {
        _respawnController = ServiceLocator.Instance.GetService<RespawnController>();
        if (_respawnController == null) {
            Debug.LogWarning("[RespawnZone] No RespawnController service registered.");
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
        if (_respawnController != null && position != null) {
            _respawnController.UpdateSpawnPoint(position);
        }
    }
}
