using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlimeAttack_JumpShot : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private SpawnRing spawnRing;
  [SerializeField] private Transform physicsTransform;
  private AudioSource audioSource;
  [SerializeField] private AudioClip FireSound;

  [Header("Shoot")]
  [SerializeField] private int bulletCount = 12;
  [SerializeField] private float spreadLen = 2f;

  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  private void ShootRadial()
  {
    audioSource.Stop();
    audioSource.time = 0.1f;
    audioSource.clip = FireSound;
    audioSource.Play();
    StartCoroutine(spawnRing.Spawn(bulletCount, spreadLen, physicsTransform.position, 0));
  }

}
