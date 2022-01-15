using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredFall : MonoBehaviour
{
    // acceleration for the gravity after falling is triggered; defaults to gravity value from physics engine
    [SerializeField]
    private Vector3 acceleration = Physics.gravity;

    [SerializeField]
    // time, in seconds, before falling begins
    private float delay = 0;

    // time, in seconds, before the objects are destroyed after falling
    [SerializeField]
    private float destroyDelay = 3;

    // if true, this object will destroy itself when destroying the falling objects
    [SerializeField]
    private bool destroySelf = true;

    [SerializeField]
    private GameObject fallingObject;

    // tag for objects that are allowed to trigger the fall
    [SerializeField]
    private string triggeringTag = "Player";

    // rigid body to fall away
    private Rigidbody _rigidbody;

    private bool _triggered;

    private void Start() {
        _rigidbody = fallingObject.GetComponent<Rigidbody>();
        if (_rigidbody == null) {
            _rigidbody = fallingObject.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (_triggered) {
            return;
        }

        if (other.gameObject.CompareTag(triggeringTag)) {
            _triggered = true;
            StartCoroutine(FallAway());
        }
    }

    private IEnumerator FallAway() {
        if (delay > 0) {
            yield return new WaitForSeconds(delay);
        }

        var timeElapsed = 0F;
        while (timeElapsed < destroyDelay) {
            timeElapsed += Time.fixedDeltaTime;

            _rigidbody.velocity += acceleration;
            _rigidbody.transform.position += _rigidbody.velocity * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        Destroy(fallingObject);

        if (destroySelf) {
            Destroy(gameObject);
        }
    }
}
