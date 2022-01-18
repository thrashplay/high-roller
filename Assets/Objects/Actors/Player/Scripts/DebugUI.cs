using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    // game object containing all UI elements
    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private Text stateText;

    [SerializeField]
    private Text terrainText;

    [SerializeField]
    private PlayerConfigModule playerConfig;

    [SerializeField]
    private PlayerState playerState;

    void Update()
    {
        ui.SetActive(playerConfig.Debug);

        stateText.text = playerState.State.ToString() + "," + playerState.MovementState.Name;
        terrainText.text = playerState.CurrentTerrain.name;
    }
}
