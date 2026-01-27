using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBulletBehavior : BulletBehavior
{
  [SerializeField] SpawnRing spawnRing;
  [SerializeField] private int burstBulletCount;
  [SerializeField] private float burstTime;

  protected override void OnStart()
  {
    base.OnStart();
    Invoke(nameof(HandleBulletDestroy), burstTime);
  }

  protected override void HandleBulletDestroy()
  {
    FindObjectOfType<CameraShake>().Shake(0.12f, 0.15f);
    StartCoroutine(spawnRing.Spawn(burstBulletCount, 0.1f, transform.position));
    Destroy(gameObject);
  }
}
