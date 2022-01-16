using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    private LevelData level;

    // tag for objects that are allowed to trigger the fall
    [SerializeField]
    private string triggeringTag = "Player";
    
    // text area to display zone name in
    [SerializeField]
    private Text zoneNameText;

    private GameController _gameController;

    private void Start() {
        var controllerObject = GameObject.FindGameObjectWithTag("GameController");
        _gameController = controllerObject.GetComponent<GameController>();

        zoneNameText.text = level.Description;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(triggeringTag)) {
            _gameController.LoadLevel(level);
        }
    }
}
