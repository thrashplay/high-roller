using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDeathController : MonoBehaviour
{
    private Rigidbody _body;

    private FallDetector _fallDetector;

    // height of the player when they started falling
    private float _initialHeight;

    // emitted when the player has fallen too far and needs to respawn
    [SerializeField]
    private Trigger _respawnTrigger;

    // emitted when the player begins to fall
    [SerializeField]
    private Trigger _fallingTrigger;

    // emitted when the player lands
    [SerializeField]
    private Trigger _landingTrigger;

    [SerializeField]
    private PlayerConfigModule _playerConfig;

    void Start() {
        _body = GetComponent<Rigidbody>();
        _fallDetector = GetComponent<FallDetector>();

        _fallingTrigger.AddListener(OnFalling);
        _landingTrigger.AddListener(OnLanding);    
    }

    private void FixedUpdate() {
        if (_fallDetector.Falling) {
            var fallDistance = _initialHeight - _body.position.y;
            if (fallDistance >= _playerConfig.EndlessFallHeight) {
                Respawn();
            }
        }
    }

    private void OnDestroy() {
        _fallingTrigger.RemoveListener(OnFalling);
        _landingTrigger.RemoveListener(OnLanding);
    }

    private void OnFalling() {
        _initialHeight = _body.position.y;
    }

    private void OnLanding() {
        if (_initialHeight - _body.position.y > _playerConfig.SafeFallDistance) {
            Respawn();
        }
    }

    private void Respawn() {
        Destroy(gameObject);
        _respawnTrigger.Emit();
    }
}
