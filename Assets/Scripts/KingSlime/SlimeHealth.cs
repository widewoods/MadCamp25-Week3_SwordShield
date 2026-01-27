using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHealth : EnemyHealth
{
  [SerializeField] private SlimeBossController slimeBossController;

  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
    if (currentHealth <= 0)
    {
      slimeBossController.Die();
    }
  }
}
