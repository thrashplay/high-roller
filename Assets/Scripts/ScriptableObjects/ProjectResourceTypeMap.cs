using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "resource_type_map", menuName = "ScriptableObjects/System/Resource Type Map", order = 1)]
public class ProjectResourceTypeMap : AbstractResourceTypeMap
{
    public override Dictionary<string, System.Type> ResourceTypes { 
        get {
            return new Dictionary<string, System.Type> {
                { "Levels", typeof(LevelData) },
                { "TerrainTypes", typeof(TerrainType) },
            };
        }
    }
}
