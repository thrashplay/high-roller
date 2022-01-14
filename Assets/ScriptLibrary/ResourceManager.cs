using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractResourceTypeMap : ScriptableObject {
    // retrieves a dictionary mapping a resource path (relative to Resources) to C# types stored at that path
    public abstract Dictionary<string, System.Type> ResourceTypes { get; }
}

[System.Serializable]
public struct ResourceReference
{
    public string id;

    public string path;
}

// adapted from: https://forum.unity.com/threads/workflow-for-locating-scriptableobjects-at-runtime.502610/
public class ResourceManager : ScriptableObject
{
    private const string MANAGER_RESOURCE_PATH = "System/resource_manager";

    [SerializeField]
    private AbstractResourceTypeMap resourceTypeMap;

    [SerializeField]
    private List<ResourceReference> resources;

    private IDictionary<string, ResourceReference> _resourceMap = new Dictionary<string, ResourceReference>();

    public static ResourceManager GetInstance() {
        var manager = Resources.Load<ResourceManager>(MANAGER_RESOURCE_PATH);
        
#if UNITY_EDITOR
        // lazily create a manager in the editor
        if (manager == null)
        {
            manager = CreateInstance<ResourceManager>();
            UnityEditor.AssetDatabase.CreateAsset(manager, "Assets/Resources/" + MANAGER_RESOURCE_PATH + ".asset");
        }
#endif

        // no manager exists, and we are in the player.. the game's probably not going to work
        if (manager == null) {
            Debug.LogError("No ResourceManager found. Using an empty one, which will probably result in missing data.");
            manager = CreateInstance<ResourceManager>();
        }

        return manager;
    }

    public T Load<T>(string id) where T : Object
    {
        if (_resourceMap.TryGetValue(id, out ResourceReference resource)) {
            return Resources.Load<T>(resource.path);
        }
        return null;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitialize() {
        var manager = GetInstance();
        manager.BuildResourceMap();
    }

    private void BuildResourceMap() {
        _resourceMap = new Dictionary<string, ResourceReference>();
        foreach (var resource in resources) {
            if (_resourceMap.ContainsKey(resource.id)) {
                Debug.LogErrorFormat("Duplicate resource id: {0}", resource.id);
                continue;
            }

            _resourceMap.Add(resource.id, resource);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void EditorInitialize() {
        var manager = GetInstance();
        manager.FindResources();
    }

    private void FindResources() {
        if (resourceTypeMap == null) {
            Debug.LogWarning("No resource type map set in editor. Resources will not be managed.");
            return;
        }

        resources = new List<ResourceReference>();
 
        foreach (var entry in resourceTypeMap.ResourceTypes) {
            var allResources = Resources.LoadAll(entry.Key, entry.Value);
            for (int i = 0; i < allResources.Length; i++)
            {
                Object resource = allResources[i];
                resources.Add(new ResourceReference() {
                    id = resource.name,
                    path = GetRelativeResourcePath(UnityEditor.AssetDatabase.GetAssetPath(resource))
                });
            }
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }

    public static string GetRelativeResourcePath(string path)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        if (path.Contains("/Resources/"))
        {
            string[] rSplit = path.Split(new string[] { "/Resources/" }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] split = rSplit[1].Split('.');
            for (int j = 0; j < split.Length - 1; j++)
            {
                stringBuilder.Append(split[j]);
                if (j < split.Length - 2)
                    stringBuilder.Append('/');
            }
            return stringBuilder.ToString();
        }
        return path;
    }

    private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false) {
            EditorInitialize();
        }
    }
#endif
}
