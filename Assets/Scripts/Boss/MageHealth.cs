using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHealth : EnemyHealth
{
  [SerializeField] MageController mageController;
  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
  }

  public override void Stunned()
  {
    mageController.Stun();
  }
}
