using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  [SerializeField] private int health = 1;

  [Header("I-Frames")]
  public float iFrameDuration = 0.8f;
  public float blinkInterval = 0.08f;

  public bool useLayerIgnore = false;
  public int playerLayer = 6;
  public int enemyLayer = 7;

  private Coroutine iFrameCo;

  [Header("Refs")]
  [SerializeField] private SpriteRenderer sr;
  [SerializeField] private HitVignetteFX hitFX;
  [SerializeField] private HeartHUD hpui;

  private bool isInvincible = false;
  private bool isAttacking = false;

  void Start()
  {
    if (!sr) sr = GetComponentInChildren<SpriteRenderer>();
  }

  protected virtual void HandleTrigger(Collider2D collision)
  {
    if (isAttacking && collision.gameObject.CompareTag("Enemy"))
    {
      GameObject hit = collision.gameObject;

      EnemyHealth enemyHealth;
      if (hit.TryGetComponent(out enemyHealth))
      {
        enemyHealth.TakeDamage(1);
      }
      else
      {
        Debug.Log("Enemy health script not found");
      }
      return;
    }
    if (collision.gameObject.CompareTag("Enemy"))
    {
      TakeDamage(1);
    }
    if (collision.gameObject.CompareTag("Bullet"))
    {
      BulletBehavior bulletBehavior;
      if (collision.gameObject.TryGetComponent(out bulletBehavior))
      {
        bulletBehavior.HandleTrigger(gameObject);
      }
      else
      {
        Debug.Log("Enemy health script not found");
      }
      Destroy(collision.gameObject);
      TakeDamage(1);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    HandleTrigger(collision);
  }

  public void SetAttacking(bool state)
  {
    isAttacking = state;
  }

  public void TakeDamage(int damage)
  {
    if (isInvincible || isAttacking) return;
    if (hpui != null) hpui.TakeHit();
    health -= damage;
    if (health <= 0)
    {
      Destroy(gameObject);
    }
    if (hitFX) hitFX.Flash();
    if (iFrameCo != null) StopCoroutine(iFrameCo);
    iFrameCo = StartCoroutine(IFramesRoutine());
  }

  private IEnumerator IFramesRoutine()
  {
    isInvincible = true;

    if (useLayerIgnore)
      Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

    float t = 0f;
    bool visible = true;

    while (t < iFrameDuration)
    {
      t += blinkInterval;
      visible = !visible;
      if (sr) sr.enabled = visible;
      yield return new WaitForSeconds(blinkInterval);
    }

    if (sr) sr.enabled = true;

    if (useLayerIgnore)
      Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

    isInvincible = false;
    iFrameCo = null;
  }
}
