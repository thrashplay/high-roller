using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // tag that will trigger the goal; default: Player
    [SerializeField]
    private string triggeringTag = "Player";

    // maximum speed the player can be moving and still trigger the goal
    [SerializeField]
    private float maximumTriggerSpeed = 0.01F;

    // service to notify when the goal is reached
    private RaceManager _raceManager;

    // boolean indicating if the player is in the goal or not
    private bool _inGoal = false;

    // reference to the rigid body of the last player object that triggered this goal
    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _raceManager = ServiceLocator.Instance.GetService<RaceManager>();
        if (_raceManager == null) {
            Debug.LogWarning("[Goal] No RaceManager service registered.");
        }
    }

    private void FixedUpdate() {
        if (_inGoal && _playerRigidbody != null ) {
            _playerRigidbody.angularVelocity *= 0.95F;

            var velocity = _playerRigidbody.velocity;
            _playerRigidbody.velocity = new Vector3(
                velocity.x * 0.95F,
                velocity.y,
                velocity.z * 0.95F
            );

            if (_playerRigidbody.velocity.magnitude <= maximumTriggerSpeed) {
               _raceManager.GoalReached();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        UpdateStatus(other, true);
    }

    private void OnTriggerStay(Collider other) {
        UpdateStatus(other, true);
    }

    private void OnTriggerExit(Collider other) {
        UpdateStatus(other, false);
    }

    private void UpdateStatus(Collider other, bool inGoal) {
        if (other.gameObject.CompareTag(triggeringTag)) {
            _inGoal = inGoal;
            if (inGoal) {
                _playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
            } else {
                _playerRigidbody = null;
            }
        }
    }
}
