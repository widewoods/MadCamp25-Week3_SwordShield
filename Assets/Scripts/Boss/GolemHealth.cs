using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHealth : EnemyHealth
{
  [SerializeField] private GolemController golemController;

  public override void TakeDamage(int damage)
  {
    if (!golemController.ProtectionBroken) return;
    base.TakeDamage(damage);
    if (currentHealth <= 0)
    {
      golemController.Die();
    }
    if (currentHealth <= maxHealth / 2)
    {
      golemController.Phase2();
    }
  }
}
