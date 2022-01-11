using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreefallGravityBoost : MonoBehaviour
{
    private Rigidbody _body;

    private IGroundDetector _groundDetector;

    [SerializeField]
    private PlayerConfigModule _playerConfig;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundDetector = GetComponent<IGroundDetector>();
    }

    void FixedUpdate()
    {
        if (!_groundDetector.IsOnGround && _playerConfig.FreefallGravityMultiplier != 1)
        {
            // subtract 1, since the physics engine applies 1x normal gravityh for us
            _body.AddForce(Physics.gravity * (_playerConfig.FreefallGravityMultiplier - 1), ForceMode.Acceleration); 
        }
    }
}
