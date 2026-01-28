using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBulletBehavior : BulletBehavior
{
  [SerializeField] SpawnRing spawnRing;
  [SerializeField] private int burstBulletCount;
  [SerializeField] private float burstTime;
  public static Action OnBurst;

  protected override void OnStart()
  {
    base.OnStart();
    Invoke(nameof(HandleBulletDestroy), burstTime);
  }

  protected override void HandleBulletDestroy()
  {
    OnBurst?.Invoke();
    FindObjectOfType<CameraShake>().Shake(0.2f, 0.3f);
    StartCoroutine(spawnRing.Spawn(burstBulletCount, 0.1f, transform.position));
    Destroy(gameObject);
  }
}
