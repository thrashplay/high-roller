using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    [SerializeField]
    private PlayerStateMachine _playerState;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_playerState.Winning) {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null) {
                var body = player.GetComponent<Rigidbody>();

                if (body.velocity.magnitude < 0.02) {
                    Debug.Log("You win!");
                    GameObject.Destroy(player);
                }
            }
        }
    }
}
