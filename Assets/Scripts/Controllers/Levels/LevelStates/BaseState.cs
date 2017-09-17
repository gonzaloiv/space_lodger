﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelStates {

    public class BaseState : State {

        #region Fields

        protected WaveController waveController;
        protected HUDController hudController;
        protected GameObject player;
        protected WaveData currentWaveData;

        private LevelController levelController;

        #endregion

        #region Mono Behaviour

        void Awake () {
            levelController = GetComponent<LevelController>();
            waveController = levelController.WaveController;
            hudController = levelController.HUDController;
            player = levelController.Player;
            currentWaveData = levelController.CurrentWaveData;
        }

        #endregion
  	
    }

}