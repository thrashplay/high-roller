using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfigModule playerConfig;

    // player state object
    [SerializeField]
    private PlayerState playerState;

    // services
    [SerializeField]
    private LevelManager levelManager;
  
    [SerializeField]
    private PlayerManager playerManager;
  
    [SerializeField]
    private RaceManager raceManager;

    [SerializeField]
    private StatisticsManager statisticsManager;

    private void Awake() {
        var services = ServiceLocator.Instance;
        services.Register(this);
        services.Register(levelManager);
        services.Register(playerManager);
        services.Register(raceManager);
        services.Register(statisticsManager);
    }

    // begins a race for the specified level
    public void BeginRace(LevelData level) {
        raceManager.BeginRace(level);
    }

    // quits the current race, returning to level select
    public void QuitRace() {
        raceManager.Quit();
    }

    // notify the game that the current level's goal has been reached
    public void GoalReached() {
        raceManager.GoalReached();
    }

    private void Start() {
        playerConfig.Debug = false;
        
        playerState.Reset();

        playerManager.OnPlayerDeath += (type) => raceManager.PlayerDied();
        raceManager.OnRaceCompleted += OnRaceCompleted;
        raceManager.OnRaceCompleted += statisticsManager.OnRaceComplete;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            QuitRace();
        }

        if (Input.GetKeyDown(KeyCode.Tilde)) {
            playerConfig.Debug = !playerConfig.Debug;
        }
    }

    private void OnRaceCompleted(RaceResults results) {
        levelManager.LoadLevel(ResourceManager.GetInstance().Load<LevelData>("level_select"));
    }
}
