using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHardMode : MonoBehaviour
{
    [SerializeField]
    private Vector3 position = Vector3.zero;

    // object that will be re-instantiated after triggering
    [SerializeField]
    private GameObject prefabToReset;

    // delay after triggering before resetting
    [SerializeField]
    private float delay = 5;

    // tag for objects that are allowed to trigger the fall
    [SerializeField]
    private string triggeringTag = "Player";

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(triggeringTag)) {
            StartCoroutine(PerformReset());
        }
    }

    private IEnumerator PerformReset() {
        yield return new WaitForSeconds(delay);
        Debug.Log("rest");
        Instantiate(prefabToReset, position, Quaternion.identity);
    }
}
