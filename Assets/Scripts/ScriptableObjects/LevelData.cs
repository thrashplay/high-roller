using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    // human readable name of this level
    [SerializeField]
    private string description;

    // location where the player should initially spawn
    [SerializeField]
    private Vector3 initialSpawnPoint;

    // the map prefab
    [SerializeField]
    private GameObject map;

    public string Description {
        get { return description; }
    }

    public Vector3 InitialSpawnPoint {
        get { return initialSpawnPoint; }
    }

    public GameObject Map {
        get { return map; }
    }
}
