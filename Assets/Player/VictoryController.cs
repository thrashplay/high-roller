using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryController : MonoBehaviour
{
    // the scene to load if the player finishes the course
    [SerializeField]
    private string _victoryScene;

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
                    SceneManager.LoadScene(_victoryScene);
                }
            }
        }
    }
}
