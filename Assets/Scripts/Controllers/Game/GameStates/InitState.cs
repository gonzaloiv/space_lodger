﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStates {

    public class InitState : BaseState {

        #region Public Behaviour

        public override void Enter () {
            base.Enter();
            new Board();
            GameConfig.Init(gameConfigData);
            levelController.gameObject.SetActive(false);
            inputManager.SetActive(false);
            InitScreens();
            DataManager.Init(); // Has to be called the last because of DataLoadedEvent.
        }

        public void OnDataLoadedEvent (DataLoadedEventArgs dataLoadedEventArgs) {
            if (!dataLoadedEventArgs.Leaderboard.IsFirstPlay) {
                gameController.ToMainMenuState();
            } else {
                // TODO: gameController.ToTutorialState();
                gameController.ToMainMenuState();   
            }
        }

        #endregion

        #region Protected Behaviour

        protected override void AddListeners () {
            DataManager.DataLoadedEvent += OnDataLoadedEvent;
        }

        protected override void RemoveListeners () {
            DataManager.DataLoadedEvent -= OnDataLoadedEvent;
        }

        #endregion

        #region Private Behaviour

        private void InitScreens () {
            mainMenuScreen.SetActive(false);
            levelScreen.SetActive(false);
            mainMenuScreen.SetActive(false);
            leaderboardScreen.SetActive(false);
        }

        #endregion

    }

}