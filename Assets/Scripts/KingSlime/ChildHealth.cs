using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHealth : EnemyHealth
{
  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
    if (currentHealth <= 0)
    {
      Destroy(gameObject);
    }
  }
}
