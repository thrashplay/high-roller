using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "game_settings", menuName = "ScriptableObjects/Game Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private DebugSettings debug;

    public static GameSettings Load(string path = "game_settings") {
        return Resources.Load<GameSettings>(path);
    }

    public DebugSettings Debug { 
        get {
            return debug;
        }
    }
}
