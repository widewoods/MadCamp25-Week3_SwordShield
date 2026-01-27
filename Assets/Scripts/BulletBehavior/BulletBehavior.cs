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
  public float Speed => speed;

  private void Start()
  {
    OnStart();
  }
  protected virtual void HandleBulletDestroy()
  {
    Destroy(gameObject);
  }

  protected virtual void OnStart()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.velocity = initialDirection * speed;
    Invoke(nameof(HandleBulletDestroy), 10f);
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Wall"))
    {
      HandleBulletDestroy();
    }
  }

  public virtual void HandleTrigger(GameObject other)
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
