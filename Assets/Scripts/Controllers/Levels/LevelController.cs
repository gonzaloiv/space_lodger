﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : StateMachine {

    #region Fields

    [SerializeField] private WaveController waveController;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private LevelScreenController levelScreenController;

    public WaveController WaveController { get { return waveController; } }
    public GameObject Player { get { return player; } }
    public LevelScreenController LevelScreenController { get { return levelScreenController; } }
    public WaveData CurrentWaveData { get { return currentLevelData.WavesData[currentWaveIndex]; } }

    private GameObject player;
    private LevelData currentLevelData;
    private int currentWaveIndex = 0;

    #endregion

    #region Mono Behaviour

    void Awake() {
        player = Instantiate(playerPrefab, transform);
        player.SetActive(false);
    }

    #endregion

    #region Public Behaviour

    public void InitLevel (LevelData levelData) {
        currentWaveIndex = 0;
        currentLevelData = levelData;
        ToNewLevelState();
    }

    public void ToNewLevelState () {
        ChangeState<LevelStates.NewLevelState>();
    }

    public void ToNewWaveState () {
        if (currentWaveIndex < currentLevelData.WavesData.Count()) {
            currentWaveIndex++;
            ChangeState<LevelStates.NewWaveState>();
        } else {
            EventManager.TriggerEvent(new LevelEndEvent(currentLevelData.LevelType));
            ChangeState<LevelStates.StopState>();
        }
    }

    public void ToPlayState () {
        ChangeState<LevelStates.PlayState>();
    }

    public void ToRestartState () {
        ChangeState<LevelStates.RestartState>();
    }

    public void ToStopState () {
        ChangeState<LevelStates.StopState>();
    }

    #endregion
	
}