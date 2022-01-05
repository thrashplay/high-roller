using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreefallGravityBoost : MonoBehaviour
{
    private Rigidbody _body;

    private FallDetector _fallDetector;

    [SerializeField]
    private PlayerConfigModule _playerConfig;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _fallDetector = GetComponent<FallDetector>();
    }

    void FixedUpdate()
    {
        if (_fallDetector.Falling && _playerConfig.FreefallGravityMultiplier != 1)
        {
            // subtract 1, since the physics engine applies 1x normal gravityh for us
            _body.AddForce(Physics.gravity * (_playerConfig.FreefallGravityMultiplier - 1), ForceMode.Acceleration); 
        }
    }
}
