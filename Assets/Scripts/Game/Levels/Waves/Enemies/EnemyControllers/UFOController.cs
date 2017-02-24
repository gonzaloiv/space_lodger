﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class UFOController : MonoBehaviour, IEnemy {

  #region Fields

  [SerializeField] private GameObject explosionPrefab;
  private ParticleSystem explosion;

  private Animator animator;
  private UFO ufo;

  private bool activeCollider; // TODO: repensar como controlar sólo una colisión por grupo de partículas

  #endregion

  #region Mono Behaviour

  void Awake() {
    explosion = Instantiate(explosionPrefab, transform).GetComponent<ParticleSystem>();
    animator = GetComponent<Animator>();
    ufo = new UFO();
  }

  void OnEnable() {
    activeCollider = true;
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.layer == (int) CollisionLayer.Board)
      Disable();
    if (collision.gameObject.layer == (int) CollisionLayer.Player) {
      animator.Play("Disable");
      explosion.Play();
    }
  }

  void OnParticleCollision(GameObject particle) {
    if (activeCollider && particle.layer == (int) CollisionLayer.Player) {
      animator.Play("Disable");
      explosion.Play();
      activeCollider = false;
      EventManager.TriggerEvent(new EnemyHitEvent(ufo.Score));
    }
  }

  #endregion

  #region Public Behaviour

  public void Disable() {
    gameObject.SetActive(false);
  }

  #endregion

}