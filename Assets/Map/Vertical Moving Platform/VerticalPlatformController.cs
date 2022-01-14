using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatformController : MonoBehaviour
{
    private GameObject player;

    private Vector3 lastPosition;

    private float storedBounce = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (player != null) {
            var delta = transform.position - lastPosition;
            var playerBody = player.GetComponent<Rigidbody>();

            if (playerBody.velocity.y > 5) {
                storedBounce = playerBody.velocity.y;
            }

            playerBody.velocity = new Vector3(playerBody.velocity.x, 0, playerBody.velocity.z);
            playerBody.MovePosition(new Vector3(playerBody.position.x, playerBody.position.y + delta.y, playerBody.position.z));

            if (delta.y < float.Epsilon) {
                if (storedBounce > float.Epsilon) {
                    playerBody.AddForce(1.5F * storedBounce * Vector3.up, ForceMode.VelocityChange);
                    storedBounce = 0;
                }
            }
        }


        this.lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            player = null;
        }
    }
}
