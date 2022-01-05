using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    // flag indicating if the player is currently falling
    private bool _falling;

    // emitted when the player leaves the ground
    [SerializeField]
    private Trigger _fallingTrigger;

    // emitted when the player lands on the ground
    [SerializeField]
    private Trigger _landedTrigger;

    // flag indicating if we are "on terrain" in this frame
    private bool _onTerrain = true;

    // the tag to use to identify an object we collide with is "terrain" or not
    [SerializeField]
    private string _terrainTag = "Terrain";

    public bool Falling {
        get { return _falling; }
    }

    void FixedUpdate()
    {
        // trigger events if our falling state has changed
        bool newFalling = !_onTerrain;
        if (_falling == false && newFalling == true) {
            _fallingTrigger.Emit();
        } else if (_falling == true && newFalling == false) {
            _landedTrigger.Emit();
        }

        // each update, we set our _falling flag based on whether we contacted terrain last fram or not
        _falling = newFalling;

        // clear the 'on terrain' flag, which will be set again by collision detection
        _onTerrain = false;
    }

    private void OnCollisionEnter(Collision other) 
    {
        HandleCollision(other);
    }

    private void OnCollisionStay(Collision other) 
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collision other) {
        if (other.gameObject.CompareTag(_terrainTag))
        {
            _onTerrain = true;
        }
    }
}
