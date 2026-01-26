using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR;

public class BulletBehavior : MonoBehaviour
{
  protected Rigidbody2D rb;
  protected Vector2 initialDirection;

  [SerializeField] protected float speed;

  private void Start()
  {
    OnStart();
  }
  protected virtual void HandleBulletDestroy()
  {
    Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    HandleTrigger(other);
  }

  protected virtual void OnStart()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.velocity = initialDirection * speed;
    Invoke(nameof(HandleBulletDestroy), 10f);
  }

  protected virtual void HandleTrigger(Collider2D other)
  {
    if (other.gameObject.CompareTag("Wall"))
    {
      HandleBulletDestroy();
    }
  }

  public void SetInitialDirection(Vector2 direction)
  {
    initialDirection = direction;
  }
}
