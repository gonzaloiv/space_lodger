﻿using UnityEngine;
using UnityEngine.Events;

#region Player Input Events

public class EscapeInput : UnityEvent {}
public class ReturnInput : UnityEvent {}

public class ClickInput : UnityEvent {

  public Vector2 Position { get { return position; } }
  private Vector2 position;

  public ClickInput(Vector2 position) {
    this.position = position;  
  }

}

#endregion

#region Game Mechanics Events 

public class EnemyHitEvent : UnityEvent {
	public EnemyHitEvent() {
	  Debug.Log("EnemyHitEvent");
  }
}

public class GameOverEvent : UnityEvent {
  public GameOverEvent() {
    Debug.Log("GameOverEvent");
  }
}

#endregion

#region UI Events 
#endregion