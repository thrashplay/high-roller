using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGroundDetector {
    // returns true if the player is on the ground
    public bool IsOnGround { get; }
}
