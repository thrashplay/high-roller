using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    private Text stateText;

    [SerializeField]
    private Text terrainText;

    [SerializeField]
    private PlayerState playerState;

    void Update()
    {
        stateText.text = playerState.State.ToString();
        terrainText.text = playerState.CurrentTerrain.name;
    }
}
