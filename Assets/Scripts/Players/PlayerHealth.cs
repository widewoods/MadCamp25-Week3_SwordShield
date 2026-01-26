using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  [SerializeField] private int health = 1;
  private AudioSource audioSource;
  [SerializeField] private AudioClip sword_hit;

  private bool isInvincible = false;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (isInvincible && collision.gameObject.CompareTag("Enemy"))
    {
      GameObject hit = collision.gameObject;
      audioSource.clip = sword_hit;
      audioSource.time = 0.15f;
      audioSource.Play();

      EnemyHealth enemyHealth;
      if (hit.TryGetComponent<EnemyHealth>(out enemyHealth))
      {
        enemyHealth.TakeDamage(1);
      }
      else
      {
        Debug.Log("Enemy health script not found");
      }
      return;
    }
    if (!isInvincible && (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Enemy")))
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
    health -= damage;
    if (health <= 0)
    {
      Destroy(gameObject);
    }
  }
}
