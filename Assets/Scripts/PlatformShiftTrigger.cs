using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShiftTrigger : MonoBehaviour
{
    // if true, this object will destroy itself when complet
    [SerializeField]
    private bool destroySelf = true;

    // the platform to shift
    [SerializeField]
    private GameObject platform;

    // game object whose position will be the new location of the platform
    [SerializeField]
    private GameObject target;

    // time, in seconds, over which the shift should occur
    [SerializeField]
    private float time = 1.0F;

    // tag for objects that are allowed to trigger the fall
    [SerializeField]
    private string triggeringTag = "Player";

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(triggeringTag)) {
            StartCoroutine(ShiftPlatform());
        }
    }

    private IEnumerator ShiftPlatform() {
        Vector3 start = platform.transform.position;
        Vector3 destination = target.transform.position;
        float timeElapsed = 0;

        while (timeElapsed < time)
        {
            // smooth-step interpolation (https://en.wikipedia.org/wiki/Smoothstep)
            float t = timeElapsed / time;
            t = t * t * (3f - 2f * t);

            var newPosition = Vector3.Lerp(start, destination, t);
            platform.transform.position = newPosition;
            timeElapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        platform.transform.position = destination;

        if (destroySelf) {
            Destroy(gameObject);
        }
    }
}
