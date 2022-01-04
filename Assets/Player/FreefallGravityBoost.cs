using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreefallGravityBoost : MonoBehaviour
{
    private Rigidbody _body;

    // flag indicating if we are on the ground or not
    private bool _onGround = true;

    [SerializeField]
    private PlayerConfigModule _playerConfig;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!_onGround && _playerConfig.FreefallGravityMultiplier != 1)
        {
            // subtract 1, since the physics engine applies 1x normal gravityh for us
            _body.AddForce(Physics.gravity * (_playerConfig.FreefallGravityMultiplier - 1), ForceMode.Acceleration); 
        }
        _onGround = false;
    }

    private void OnCollisionStay(Collision other) 
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            _onGround = true;
        }
    }
}
