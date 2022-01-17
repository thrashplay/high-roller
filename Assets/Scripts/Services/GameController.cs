using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfigModule playerConfig;


    // services
    private RaceManager _raceManager;
    private LevelManager _levelManager;

    private void Awake() {
        ServiceLocator.Instance.Register(this);
    }

    // begins a race for the specified level
    public void BeginRace(LevelData level) {
        _raceManager.BeginRace(level);
    }

    // quits the current race, returning to level select
    public void QuitRace() {
        _raceManager.Quit();
    }

    // notify the game that the current level's goal has been reached
    public void GoalReached() {
        _raceManager.GoalReached();
    }

    private void Start() {
        playerConfig.Debug = false;

        _levelManager = ServiceLocator.Instance.GetService<LevelManager>();
        _raceManager = ServiceLocator.Instance.GetService<RaceManager>();

        _raceManager.OnRaceCompleted.AddListener(OnRaceCompleted);
    }

    private void OnDestroy() {
        _raceManager.OnRaceCompleted.RemoveListener(OnRaceCompleted);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            QuitRace();
        }

        if (Input.GetKeyDown(KeyCode.Tilde)) {
            playerConfig.Debug = !playerConfig.Debug;
        }
    }

    private void OnRaceCompleted(RaceResults results) {
        _levelManager.LoadLevel(ResourceManager.GetInstance().Load<LevelData>("level_select"));
    }
}
