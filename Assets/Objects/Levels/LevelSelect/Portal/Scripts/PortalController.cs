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
    
    // text area to display death statistics in
    [SerializeField]
    private Text deathsText;

    // text area to display time statistics in
    [SerializeField]
    private Text timesText;

    // text area to display zone name in
    [SerializeField]
    private Text zoneNameText;

    private GameController _gameController;

    private StatisticsManager _statistics;

    private void Start() {
        _gameController = ServiceLocator.Instance.GetService<GameController>();
        _statistics = ServiceLocator.Instance.GetService<StatisticsManager>();

        zoneNameText.text = level.Description;
    }

    private void Update() {
        var statistics = _statistics.GetLevelStatistics(level.name);
        deathsText.text = string.Format("Best: {0}\nTotal: {1}", statistics.BestDeaths, statistics.TotalDeaths);
        timesText.text = string.Format("Best: {0:F1}\nTotal: {1:F1}", statistics.BestTime, statistics.TotalTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(triggeringTag)) {
            _gameController.BeginRace(level);
        }
    }
}
