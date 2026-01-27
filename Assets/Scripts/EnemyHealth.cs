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
  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;
  }

  public virtual void TakeDamage(int damage)
  {
    currentHealth -= damage;
    HitStop.Instance.Do(timeScale, durationRealtime);
    FindObjectOfType<CameraShake>().Shake(shakeDuration, shakeMagnitude);
  }

  public virtual void Stunned()
  {

  }
}
