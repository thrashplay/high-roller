using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Results for a completed race.
public class RaceResults {
    public LevelData Level { get; set; }

    // true if the goal was reached, false if the race was aborted
    public bool Success { get; set; }

    // number of player deaths during the race
    public int Deaths { get; set; }

    // how long the race was active for
    public float ElapsedTime { get; set; }
}

// Ongoing status for the current in-progress race.
public class RaceManager : MonoBehaviour
{
    public RaceCompletedTrigger OnRaceCompleted { get; private set; }

    // whether a race is currently active or not
    public bool Active { 
        get {
            return Level != null;
        }
    }

    // how many times the player died during the current race
    public int Deaths { get; set; }

    // elapsed time from when the race was started until now
    public float ElapsedTime {
        get {
            return Time.time - StartTime;
        }
    }

    // the level the race takes place on
    public LevelData Level { get; private set; }

    // game time when the race was started
    public float StartTime { get; private set; }

    private LevelManager _levelManager;

    public void BeginRace(LevelData level) {
        Reset();

        StartTime = Time.time;
        Level = level;
        _levelManager.LoadLevel(level);
    }

    public void GoalReached() {
        OnRaceCompleted.Emit(CreateResults(true));
        Reset();
    }

    public void Quit() {
        OnRaceCompleted.Emit(CreateResults(false));
        Reset();
    }

    private void Awake() {
        ServiceLocator.Instance.Register(this);
        OnRaceCompleted = ScriptableObject.CreateInstance<RaceCompletedTrigger>();
    }

    private void Start() {
        _levelManager = ServiceLocator.Instance.GetService<LevelManager>();
    }

    private RaceResults CreateResults(bool success) {
        return new RaceResults() {
            Level = Level,
            Success = success,
            Deaths = Deaths,
            ElapsedTime = ElapsedTime,
        };
    }

    private void Reset() {
        Deaths = 0;
        StartTime = 0;
        Level = null;
    }
}
