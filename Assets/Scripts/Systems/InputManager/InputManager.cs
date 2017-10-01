﻿using UnityEngine;
using System.Collections;
using PDollarGestureRecognizer;

public class InputManager : MonoBehaviour {

    #region Fields

    [SerializeField] private GameObject handPrefab;
    private HandController handController;

    [SerializeField] private GameObject resultPrefab;
    private ResultController resultController;

    private GestureRecognizer gestureRecognizer;

    private RuntimePlatform platform;
    private Vector3 virtualKeyPosition = Vector2.zero;

    private IEnumerator gestureRoutine;
    private bool listening = false;
    private bool mouseUp = true; // TODO: corregir esto con extensiones para las Coroutines
    private float sectionTime;

    #endregion

    #region Mono Behaviour

    void Awake () {
        gestureRecognizer = GetComponentInChildren<GestureRecognizer>();
        handController = Instantiate(handPrefab, transform).GetComponent<HandController>();
        resultController = Instantiate(resultPrefab, transform).GetComponent<ResultController>();
        platform = Application.platform;
    }

    void OnEnable () {
        EventManager.StartListening<EnemyAttackEvent>(OnEnemyAttackEvent);
    }

    void OnDisable () {
        gestureRecognizer.ResetGestureLines();
        handController.RemoveHand();
        EventManager.StopListening<EnemyAttackEvent>(OnEnemyAttackEvent);
    }

    void Update () {

        if (Time.timeScale != 0) { 

            // KEYBOARD

            if (Input.GetKeyDown(KeyCode.Escape))
                EventManager.TriggerEvent(new EscapeInputEvent());

            // MOUSE & TOUCH

            if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
                if (Input.touchCount > 0)
                    virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            } else {
                if (Input.GetMouseButton(0))
                    virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonDown(0)) {
                if (!listening)
                    StartCoroutine(GestureRoutine(sectionTime));
                gestureRecognizer.NewLine(transform);
                mouseUp = false;
            }

            if (Input.GetMouseButton(0)) {
                handController.SetHand(0, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
                gestureRecognizer.NewPoint(virtualKeyPosition);
            }

            if (Input.GetMouseButtonUp(0)) {
                mouseUp = true;
                resultController.SetCursorPosition(Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }

        }

    }

    #endregion

    #region Event Behaviour

    void OnEnemyAttackEvent (EnemyAttackEvent enemyAttackEvent) {
        sectionTime = enemyAttackEvent.SectionTime;
    }

    #endregion

    #region Private Behaviour

    private IEnumerator GestureRoutine (float sectionTime) {
        
        listening = true;
        float initialTime = Time.time;

        while (!mouseUp)
            yield return null;

        yield return new WaitForSeconds(Config.GESTURE_STROKE_TIME);

        while (!mouseUp)
            yield return null;

        GestureTime gestureTime = SetGestureTime(initialTime, sectionTime);

        if (mouseUp && gestureRecognizer.CurrentPointAmount > 5) { // The library doesn't support one click inputs
            Result result = gestureRecognizer.RecognizeGesture();
            EventManager.TriggerEvent(new GestureInput(result.GestureClass.ToString(), result.Score, gestureTime));
        }

        gestureRecognizer.ResetGestureLines();

        listening = false;

    }

    private GestureTime SetGestureTime (float initialTime, float sectionTime) {
        float finalTime = Time.time - initialTime;
        if (finalTime < sectionTime) {
            return GestureTime.TooFast;
        } else if (finalTime > 5 * sectionTime) {
            return GestureTime.TooSlow; 
        } else if (finalTime > 2 * sectionTime && finalTime < 5 * sectionTime) {
            return GestureTime.Perfect; 
        } else {
            return GestureTime.Ok;
        }
    }

    #endregion

}