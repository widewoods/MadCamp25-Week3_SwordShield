using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildHealth : EnemyHealth
{
  [SerializeField] private Animator animator;
  [SerializeField] private MonoBehaviour[] scripts;
  private static int childCount;
  void Start()
  {
    childCount++;
  }
  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);
    if (currentHealth <= 0)
    {
      audioSource.PlayOneShot(deathSound);
      animator.SetTrigger("Death");
      for (int i = 0; i < scripts.Length; i++)
      {
        scripts[i].enabled = false;
        scripts[i].StopAllCoroutines();
      }
      GetComponent<Rigidbody2D>().velocity = Vector2.zero;
      GetComponent<Rigidbody2D>().angularVelocity = 0;
      GetComponent<CircleCollider2D>().enabled = false;
      Destroy(transform.GetChild(0).gameObject);
      transform.up = Vector2.up;
      childCount--;
      if (childCount <= 0)
      {
        CallBossDeath();
      }
    }
  }
}
