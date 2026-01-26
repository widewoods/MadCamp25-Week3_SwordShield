using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBulletBehavior : BulletBehavior
{
  [SerializeField] SpawnRing spawnRing;
  [SerializeField] private int burstBulletCount;
  [SerializeField] private float burstTime;
  protected override void HandleBulletDestroy()
  {
    StartCoroutine(spawnRing.Spawn(burstBulletCount, 0.1f, transform.position));
    Destroy(gameObject);
  }
}
