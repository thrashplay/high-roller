using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelStatistics
{
    public int Attempts { get; set; }
    public int Completions { get; set; }

    public int BestDeaths { get; set; }
    public int TotalDeaths { get; set; }

    public float BestTime { get; set; }
    public float TotalTime { get; set; }
}

public class StatisticsManager : MonoBehaviour
{
    private readonly IDictionary<string, LevelStatistics> _statisticsMap = new Dictionary<string, LevelStatistics>();

    public LevelStatistics GetLevelStatistics(string levelId) {
        if (!_statisticsMap.TryGetValue(levelId, out LevelStatistics statistics)) {
            statistics = new LevelStatistics();
            _statisticsMap[levelId] = statistics;
        }

        return statistics;
    }

    public void OnRaceComplete(RaceResults results) {
        var statistics = GetLevelStatistics(results.Level.name);

        // set any new records, if the level was finished
        if (results.Success) {
            if (statistics.Completions == 0 || results.ElapsedTime < statistics.BestTime) {
                statistics.BestTime = results.ElapsedTime;
            }
            
            if (statistics.Completions == 0 || results.Deaths < statistics.BestDeaths) {
                statistics.BestDeaths = results.Deaths;
            }

            statistics.Completions++;
        }

        statistics.Attempts++;
        statistics.TotalTime += results.ElapsedTime;
        statistics.TotalDeaths += results.Deaths;
    }
}
