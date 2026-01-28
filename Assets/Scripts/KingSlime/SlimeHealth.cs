using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHealth : EnemyHealth
{
  [SerializeField] private SlimeBossController slimeBossController;

  public override void TakeDamage(int damage)
  {
    if (slimeBossController.isInvincible) return;
    base.TakeDamage(damage);
    if (currentHealth <= 0)
    {
      slimeBossController.Die();
      slimeBossController.StopAllCoroutines();
      audioSource.PlayOneShot(deathSound);
    }
  }
}
