using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  [SerializeField] private int maxHealth = 1;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Bullet"))
    {
      Debug.Log("Hit!");
      TakeDamage(1);
    }
  }

  public void TakeDamage(int damage)
  {
    maxHealth -= damage;
  }
}
