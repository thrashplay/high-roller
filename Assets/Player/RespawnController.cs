using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    // initial position for newly spawned players
    [SerializeField]
    private Vector3 _initialPosition;

    // the prefab representing the player object
    [SerializeField]
    private GameObject _playerPrefab;

    // trigger emitted when the player should respawn
    [SerializeField]
    private Trigger _respawnTrigger;

    void Start()
    {
        _respawnTrigger.AddListener(OnRespawn);
        OnRespawn();
    }

    private void OnDestroy() {
        _respawnTrigger.RemoveListener(OnRespawn);
    }

    private void OnRespawn()
    {
        Instantiate(_playerPrefab, _initialPosition, Quaternion.identity);
    }
}
