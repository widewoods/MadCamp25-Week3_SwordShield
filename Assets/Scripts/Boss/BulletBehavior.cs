using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
  private void Start()
  {
    Invoke("DestroyBullet", 5f);
  }
  private void DestroyBullet()
  {
    Destroy(gameObject);
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Wall"))
    {
      Destroy(gameObject);
    }
  }
}
