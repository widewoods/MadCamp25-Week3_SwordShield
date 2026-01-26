using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpiral : MonoBehaviour
{
  [SerializeField] private GameObject bulletPrefab;
  // Start is called before the first frame update
  void Start()
  {

  }


  public IEnumerator Spawn(int bulletCount, float ringRadius, Vector3 centerPosition)
  {
    float angleStep = 360f / bulletCount;
    float angle = 0f;

    for (int i = 0; i < bulletCount; i++)
    {
      float bulletPositionX = centerPosition.x + Mathf.Sin(angle * (Mathf.PI / 180)) * ringRadius;
      float bulletPositionY = centerPosition.y + Mathf.Cos(angle * (Mathf.PI / 180)) * ringRadius;

      Vector3 bulletVector = new Vector3(bulletPositionX, bulletPositionY, 0f);
      Vector3 bulletMoveDirection = (bulletVector - centerPosition).normalized;

      GameObject bullet = Instantiate(bulletPrefab, bulletVector, Quaternion.identity);
      BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
      bulletBehavior.SetInitialDirection(bulletMoveDirection);

      angle += angleStep;
      yield return new WaitForSeconds(0.5f / bulletCount);
    }
  }
}
