using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : EnemyHealth
{
  public static int towerCount = 0;
  public static event Action OnTowersBroken;
  [SerializeField] private GameObject particlePrefab;
  // Start is called before the first frame update
  void Start()
  {
    towerCount++;
  }


  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
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
