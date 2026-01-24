using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  [SerializeField] private int maxHealth = 1;
  private bool isInvincible = false;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (isInvincible) return;
    if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Enemy"))
    {
      Debug.Log("Hit!");
      TakeDamage(1);
    }
  }

  public void SetInvincible(bool state)
  {
    isInvincible = state;
  }

  public void TakeDamage(int damage)
  {
    maxHealth -= damage;
  }
}
