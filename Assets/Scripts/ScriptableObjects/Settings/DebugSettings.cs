using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebugSettings
{
    [SerializeField]
    private bool enabled;

    [SerializeField]
    private bool showRespawn;

    public bool Enabled {
        get { return enabled; }
        set { enabled = value; }
    }

    public bool ShowRespawn {
        get { return Enabled && showRespawn; }
        set { showRespawn = value; }
    }
}
