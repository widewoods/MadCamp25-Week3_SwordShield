using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
  [SerializeField] private SpawnRing spawnRing;
  [SerializeField] private GolemController golemController;


  [SerializeField] private float shootInterval = 1f;
  private float shootTimer = 0f;

  void Start()
  {
    spawnRing = GetComponent<SpawnRing>();
  }

  void FixedUpdate()
  {
    shootTimer += Time.fixedDeltaTime;
    if (shootTimer >= shootInterval)
    {
      shootTimer = 0f;
      StartCoroutine(spawnRing.Spawn(4, 1.5f, transform.position, UnityEngine.Random.Range(0, 360f)));
    }

  }
}
