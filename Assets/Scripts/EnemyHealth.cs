using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
  [SerializeField] protected int maxHealth;
  protected int currentHealth;
  public int CurrentHealth => currentHealth;

  [SerializeField] private float timeScale;
  [SerializeField] private float durationRealtime;
  [SerializeField] private float shakeDuration;
  [SerializeField] private float shakeMagnitude;
  [SerializeField] protected AudioClip hitSound;
  [SerializeField] protected AudioClip deathSound;
  protected AudioSource audioSource;
  private SpriteRenderer spriteRenderer;


  void Awake()
  {
    currentHealth = maxHealth;
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
  }

  IEnumerator Blink()
  {
    spriteRenderer.color = Color.red;
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.color = Color.white;
  }

  public virtual void TakeDamage(int damage)
  {
    currentHealth -= damage;
    audioSource.PlayOneShot(hitSound);
    StartCoroutine(Blink());
    HitStop.Instance.Do(timeScale, durationRealtime);
    FindObjectOfType<CameraShake>().Shake(shakeDuration, shakeMagnitude);
  }

  public virtual void Stunned()
  {

  }
}
