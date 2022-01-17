using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelStatistics
{
    public int BestDeaths { get; set; }
    public int TotalDeaths { get; set; }

    public float BestTime { get; set; }
    public float TotalTime { get; set; }
}

public class StatisticsService : MonoBehaviour, ILevelChangeListener
{
    private readonly IDictionary<string, LevelStatistics> _statisticsMap = new Dictionary<string, LevelStatistics>();

    private LevelManager _levelManager;

    private float _levelLoadedAt;

    private void Awake() {
        ServiceLocator.Instance.Register(this);
    }

    private void Start() {
        _levelManager = ServiceLocator.Instance.GetService<LevelManager>();
        _levelManager.AddListener(this);
    }

    private void OnDestroy() {
        _levelManager.RemoveListener(this);
        ServiceLocator.Instance.Unregister(this);
    }

    public LevelStatistics GetLevelStatistics(string levelId) {
        if (!_statisticsMap.TryGetValue(levelId, out LevelStatistics statistics)) {
            statistics = new LevelStatistics();
            _statisticsMap[levelId] = statistics;
        }

        return statistics;
    }

    public void OnLevelLoad(LevelData level) {
        _levelLoadedAt = Time.time;
    }


    public void OnLevelUnload(LevelData level) {
        var elapsed = Time.time - _levelLoadedAt;
        GetLevelStatistics(level.name).TotalTime += elapsed;
    }
}
