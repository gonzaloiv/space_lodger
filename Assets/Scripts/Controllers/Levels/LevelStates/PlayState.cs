﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LevelStates {

    public class PlayState : BaseState {

        #region Fields

        private GameObject currentEnemy;
        private GameObject previousEnemy;
        private IEnumerator waveRoutine;
        private bool playing = false;

        #endregion

        #region State Behaviour

        public override void Enter () {
            previousEnemy = null;
            waveRoutine = WaveRoutine();
            StartCoroutine(waveRoutine);
        }

        public override void Play () {
            if (!playing && waveController.CurrentWaveEnemyObjects.Where(x => x.activeSelf).Count() > 0) {
                StopCoroutine(waveRoutine);
                waveRoutine = WaveRoutine();
                StartCoroutine(waveRoutine);
            }
        }

        #endregion

        #region Mono Behaviour

        void OnEnable () {
            EventManager.StartListening<PlayerHitEvent>(OnPlayerHitEvent);
        }

        void OnDisable () {
            EventManager.StopListening<PlayerHitEvent>(OnPlayerHitEvent);
        }

        #endregion

        #region Event Behaviour

        void OnPlayerHitEvent (PlayerHitEvent playerHitEvent) {
            StopCoroutine(waveRoutine);
        }

        #endregion

        #region Private Behaviour

        private IEnumerator WaveRoutine () {

            float routineTime = Random.Range(currentWaveData.RoutineTime[0], currentWaveData.RoutineTime[1]);
            playing = true;

            yield return new WaitForSeconds(1);
            currentEnemy = waveController.CurrentWaveEnemyObjects[Random.Range(0, waveController.CurrentWaveEnemyObjects.Count)];
            currentEnemy.GetComponent<IEnemyBehaviour>().Play(routineTime);
            previousEnemy = currentEnemy;

            yield return new WaitForSeconds(routineTime);
            playing = false;

        }

        private void SetCurrentEnemy () {
            if (waveController.CurrentWaveEnemyObjects.Count == 1) {
                currentEnemy = waveController.CurrentWaveEnemyObjects[0];
            } else {
                currentEnemy = previousEnemy;
                while (currentEnemy == previousEnemy) {
                    currentEnemy = waveController.CurrentWaveEnemyObjects[Random.Range(0, waveController.CurrentWaveEnemyObjects.Count)];
                }
            }
        }

        #endregion

    }


}