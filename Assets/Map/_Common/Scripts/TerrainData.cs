using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITerrainData {
    TerrainType TerrainType { get; }
}

public class TerrainData : MonoBehaviour, ITerrainData
{
    public static readonly ITerrainData DEFAULT = new DefaultTerrainData();

    [SerializeField]
    private TerrainType terrainType;

    // Retrieves terrain data from the given terrain object, returning a default instance if none
    public static ITerrainData FromGameObject(GameObject gameObject) {
        var data = gameObject.GetComponent<ITerrainData>();
        return data == null ? DEFAULT : data;
    }

    // retrieves the type of terrain
    public TerrainType TerrainType {
        get { return terrainType; }
    }

    private class DefaultTerrainData : ITerrainData {
        public TerrainType TerrainType {
            get { return ResourceManager.GetInstance().Load<TerrainType>("default"); }
        }
    }
}
