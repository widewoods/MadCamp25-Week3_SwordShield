using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
  [SerializeField] private SpawnRing spawnRing;
  [SerializeField] private GameObject particlePrefab;
  [SerializeField] private GolemController golemController;

  public static int towerCount = 0;
  private float shootInterval = 2f;
  private float shootTimer = 0f;
  public static event Action OnTowersBroken;

  void Start()
  {
    spawnRing = GetComponent<SpawnRing>();
    towerCount++;
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

  public void Break()
  {
    towerCount--;
    GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
    particle.GetComponent<ParticleSystem>().Play();
    if (towerCount <= 0)
    {
      OnTowersBroken?.Invoke();
    }
    Destroy(gameObject);
  }
}
