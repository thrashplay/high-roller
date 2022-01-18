using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampAccelerate : MonoBehaviour
{
    [SerializeField]
    private float VelocityMultiplier = 2.5F;

    private void OnTriggerEnter(Collider other) {
        var body = other.gameObject.GetComponent<Rigidbody>();
        if (body != null) {
            if (body.velocity.y < 0) {
                // TODO: move to configuration
                body.velocity *= VelocityMultiplier;
            }
        }
    }
}
