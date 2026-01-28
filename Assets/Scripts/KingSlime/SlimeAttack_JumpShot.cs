using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlimeAttack_JumpShot : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private SpawnRing spawnRing;
  [SerializeField] private Transform physicsTransform;

  [Header("Shoot")]
  [SerializeField] private int bulletCount = 12;
  [SerializeField] private float spreadLen = 2f;

  private void ShootRadial()
  {
    StartCoroutine(spawnRing.Spawn(bulletCount, spreadLen, physicsTransform.position, 0));
  }

}
