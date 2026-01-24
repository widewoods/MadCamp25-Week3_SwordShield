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

      if (hit.name == "Golem_Prefab")
      {
        BossHealth bossHealth = hit.GetComponent<BossHealth>();
        bossHealth.DamageBoss(1);
        HitStop.Instance.Do(0.06f, 0.6f);
        FindObjectOfType<CameraShake>().Shake(0.2f, 0.3f);
      }
      if (hit.name == "Minion_Tower")
      {
        Debug.Log("Tower hit");
        MinionController tower = hit.GetComponent<MinionController>();
        tower.Break();
        HitStop.Instance.Do(0.03f, 0.2f);
        FindObjectOfType<CameraShake>().Shake(0.1f, 0.01f);
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
