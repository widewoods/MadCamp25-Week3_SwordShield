using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArc : MonoBehaviour
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

  public IEnumerator Spawn(int bulletCount, float baseAngle, Vector3 centerPosition, float arcDegrees)
  {
    float angleStep = arcDegrees / bulletCount;
    float angle = baseAngle;

    for (int i = 0; i < bulletCount; i++)
    {
      float bulletPositionX = centerPosition.x + Mathf.Sin(angle * (Mathf.PI / 180));
      float bulletPositionY = centerPosition.y + Mathf.Cos(angle * (Mathf.PI / 180));

      Vector3 bulletVector = new Vector3(bulletPositionX, bulletPositionY, 0f);
      Vector3 bulletMoveDirection = (bulletVector - centerPosition).normalized;

      float deg = Mathf.Atan2(bulletMoveDirection.y, bulletMoveDirection.x) * Mathf.Rad2Deg;
      Quaternion rot = Quaternion.Euler(0f, 0f, deg - 90f);
      GameObject bullet = Instantiate(bulletPrefab, bulletVector, rot);
      BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
      bulletBehavior.SetInitialDirection(bulletMoveDirection);

      angle += angleStep;
      yield return new WaitForSeconds(0.02f);
    }
    yield return null;
  }
}
