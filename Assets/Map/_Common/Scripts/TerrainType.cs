using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainType", menuName = "ScriptableObjects/TerrainType", order = 1)]
public class TerrainType : ScriptableObject
{
    public float friction = 0;

    // whether it is safe to fall into this terrain from any height
    public bool SafeToFall = false;

    // factor to adjust velocities by while in this terrain
    public float VelocityMultiplier = 1;
}
