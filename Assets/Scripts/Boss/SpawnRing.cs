using System.Collections;
using UnityEngine;

public class SpawnRing : MonoBehaviour
{
  [SerializeField] private GameObject bulletPrefab;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public IEnumerator Spawn(int bulletCount, float ringRadius, Vector3 centerPosition, float rotation = 0f)
  {
    float angleStep = 360f / bulletCount;
    float angle = rotation;

    for (int i = 0; i < bulletCount; i++)
    {
      float bulletPositionX = centerPosition.x + Mathf.Sin(angle * (Mathf.PI / 180)) * ringRadius;
      float bulletPositionY = centerPosition.y + Mathf.Cos(angle * (Mathf.PI / 180)) * ringRadius;

      Vector3 bulletVector = new Vector3(bulletPositionX, bulletPositionY, 0f);
      Vector3 bulletMoveDirection = (bulletVector - centerPosition).normalized;

      float deg = Mathf.Atan2(bulletMoveDirection.y, bulletMoveDirection.x) * Mathf.Rad2Deg;
      Quaternion rot = Quaternion.Euler(0f, 0f, deg - 90f);
      GameObject bullet = Instantiate(bulletPrefab, bulletVector, rot);
      Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
      rb.velocity = bulletMoveDirection * 5f;

      angle += angleStep;
    }
    yield return null;
  }
}
