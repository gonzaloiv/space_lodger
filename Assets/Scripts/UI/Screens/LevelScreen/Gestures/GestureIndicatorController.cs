﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureIndicatorController : MonoBehaviour {

    #region Fields

    [SerializeField] private GameObject gestureLineRendererPrefab;

    private GameObjectPool gestureLineRendererPool;
    private Camera cam;
    private List<LineRenderer> gestureLinesRenderers = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;

    #endregion

    #region Mono Behaviour

    void Awake () {
        gestureLineRendererPool = new GameObjectPool("GesturePool", gestureLineRendererPrefab, 2, transform);
    }

    #endregion

    #region Public Behaviour

    public void Init (Camera camera) {
        this.cam = camera;   
    }

    public void SpawnGestureLineRenderer (Vector2 virtualInitialPosition) {

        Vector2 initialPosition = cam.ScreenToWorldPoint(new Vector3(virtualInitialPosition.x, virtualInitialPosition.y, 1));
            
        GameObject gesture = gestureLineRendererPool.PopObject();
        gesture.transform.position = transform.position;
        gesture.transform.rotation = transform.rotation;
        gesture.SetActive(true);

        LineRenderer gestureLineRenderer = gesture.GetComponent<LineRenderer>();
        gestureLineRenderer.positionCount = gestureLineRenderer.positionCount == 0 ? gestureLineRenderer.positionCount + 1 : gestureLineRenderer.positionCount;
        gestureLineRenderer.SetPosition(0, initialPosition);

        gestureLinesRenderers.Add(gestureLineRenderer);
        currentGestureLineRenderer = gestureLineRenderer;

    }

    public void SetNewPosition (Vector2 virtualPosition) {
        Vector2 position = cam.ScreenToWorldPoint(new Vector3(virtualPosition.x, virtualPosition.y, 1));
        currentGestureLineRenderer.positionCount++;
        currentGestureLineRenderer.SetPosition(currentGestureLineRenderer.positionCount - 1, position);
    }

    public void ResetGestureLines () {
        foreach (LineRenderer lineRenderer in gestureLinesRenderers) {
            lineRenderer.positionCount = 0;
            lineRenderer.gameObject.SetActive(false);
        }
        gestureLinesRenderers.Clear();
    }

    #endregion
	
}
