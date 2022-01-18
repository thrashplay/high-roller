using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FallingSettings
{
    [SerializeField]
    [Tooltip("Amount of gravity to apply to a falling player (but not when on slopes or ground)")]
    public Vector3 Gravity = 9.81F * Vector3.down;
    
    [SerializeField]
    [Tooltip("Maximum height that the player can fall before they are despawned")]
    public float MaxFreefall;
    
    [SerializeField]
    [Tooltip("Maximum height the player can fall from without shattering")]
    public float SafeFallHeight = 2.2F;
}

[CreateAssetMenu(fileName = "player_settings", menuName = "ScriptableObjects/Player Settings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [SerializeField]
    private FallingSettings falling;

    public FallingSettings Falling {
        get {
            return falling;
        }
    }
}
