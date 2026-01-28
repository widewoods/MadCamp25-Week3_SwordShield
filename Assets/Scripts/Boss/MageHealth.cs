using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHealth : EnemyHealth
{
  [SerializeField] MageController mageController;
  [SerializeField] AudioClip phaseTwoSound;
  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
    if (currentHealth <= 0)
    {
      audioSource.PlayOneShot(deathSound);
      mageController.Die();
      Destroy(transform.GetChild(0).gameObject);
      GetComponent<BoxCollider2D>().enabled = false;
    }
    if (currentHealth <= maxHealth / 2)
    {
      audioSource.PlayOneShot(phaseTwoSound);
      mageController.EnterPhase2();
    }
  }

  public override void Stunned()
  {
    mageController.Stun();
  }
}
