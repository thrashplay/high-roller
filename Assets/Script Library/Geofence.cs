using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geofence : MonoBehaviour
{
    // emitted when the zone is entered
    [SerializeField]
    private Trigger enterTrigger = null;

    // emitted when the zone is exited
    [SerializeField]
    private Trigger exitTrigger = null;

    // true if the zone has been entered but not exited
    [SerializeField]
    private BooleanValue inZone = null;

    // the Tag added to the objects for which enter/exit events are tracked
    [SerializeField]
    private string tagToWatch;

    // clears the inZone value, if (for example) the player dies in a zone
    public void ClearPresence() {
        inZone.Value = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(tagToWatch)) {
            Debug.Log("enter;");
            if (enterTrigger != null) {
                enterTrigger.Emit();
            }

            if (inZone != null) {
                inZone.Value = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag(tagToWatch)) {
            if (exitTrigger != null) {
                exitTrigger.Emit();
            }

            if (inZone != null) {
                inZone.Value = false;
            }
        }
    }
}
