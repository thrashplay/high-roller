using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geofence : MonoBehaviour
{
    [SerializeField]
    private bool debug = false;

    // emitted when the zone is entered
    [SerializeField]
    private TriggerWithGameObject enterTrigger = null;

    // emitted when the zone is exited
    [SerializeField]
    private TriggerWithGameObject exitTrigger = null;

    // true if the zone has been entered but not exited
    [SerializeField]
    private BooleanValue inZone = null;

    // the Tag added to the objects for which enter/exit events are tracked
    [SerializeField]
    private string tagToWatch;

    // if we are in a triggered state, this is the game object it is with; null otherwise
    private GameObject _triggeredByThisUpdate;

    // if we were in a triggered state last update, this is the game object it was with; null otherwise
    private GameObject _triggeredByLastUpdate;

    void FixedUpdate()
    {
        if (_triggeredByLastUpdate == null && _triggeredByThisUpdate != null) {
            // new trigger
            HandleTriggerStart(_triggeredByThisUpdate);
        } else if (_triggeredByLastUpdate != null && _triggeredByThisUpdate == null) {
            // triggered just ended
            HandleTriggerEnd(_triggeredByLastUpdate);
        }

        _triggeredByLastUpdate = _triggeredByThisUpdate;
        _triggeredByThisUpdate = null;
    }

    private void OnTriggerEnter(Collider other) {
        HandleTriggered(other);
    }

    private void OnTriggerStay(Collider other) {
        HandleTriggered(other);
    }

    private void HandleTriggered(Collider other) {
        if (other.gameObject.CompareTag(tagToWatch)) {
            _triggeredByThisUpdate = other.gameObject;
        }
    }

    private void HandleTriggerStart(GameObject gameObject) {
        if (gameObject.CompareTag(tagToWatch)) {
            if (debug) {
                Debug.LogFormat("Entering geofence: {0}", name);
            }

            if (enterTrigger != null) {
                enterTrigger.Emit(gameObject);
            }

            if (inZone != null) {
                inZone.Value = true;
            }
        }
    }

    private void HandleTriggerEnd(GameObject gameObject) {
        if (gameObject.CompareTag(tagToWatch)) {
            if (debug) {
                Debug.LogFormat("Exiting geofence: {0}", name);
            }

            if (exitTrigger != null) {
                exitTrigger.Emit(gameObject);
            }

            if (inZone != null) {
                inZone.Value = false;
            }
        }
    }
}
