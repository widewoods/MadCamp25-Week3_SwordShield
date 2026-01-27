using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSimple : MonoBehaviour
{
  public GameObject bulletPrefab;
  // Start is called before the first frame update
  void Start()
  {
    if (bulletPrefab == null)
    {
      Debug.Log("Bullet prefab null");
    }
  }

  public IEnumerator Spawn(int bulletCount, Vector3 spawnPosition, Vector2 initialDirection, float spawnTerm)
  {

    for (int i = 0; i < bulletCount; i++)
    {

      Vector3 bulletVector = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
      Vector3 bulletMoveDirection = initialDirection;


      GameObject bullet = Instantiate(bulletPrefab, bulletVector, Quaternion.identity);
      BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
      bulletBehavior.SetInitialDirection(bulletMoveDirection);

      yield return new WaitForSeconds(spawnTerm);
    }
    yield return null;
  }
}
