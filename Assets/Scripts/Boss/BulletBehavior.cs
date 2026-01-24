using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
  [SerializeField] private SpawnSpiral spawnSpiral;
  private void Start()
  {
    Invoke("DestroyBullet", 5f);
  }
  private void DestroyBullet()
  {
    Destroy(gameObject);
  }
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Wall"))
    {
      Destroy(gameObject);
    }
  }
}
