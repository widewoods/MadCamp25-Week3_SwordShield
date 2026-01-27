using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHealth : EnemyHealth
{
  [SerializeField] MageController mageController;
  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
    if (currentHealth <= 0)
    {
      mageController.Die();
    }
    if (currentHealth <= maxHealth / 2)
    {
      mageController.EnterPhase2();
    }
  }

  public override void Stunned()
  {
    mageController.Stun();
  }
}
