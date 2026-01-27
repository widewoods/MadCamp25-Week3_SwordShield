using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBulletBehavior : BulletBehavior
{
  [SerializeField] private float homingTime;
  [SerializeField] private float lockTime;
  float homingTimer = 0f;

  private Transform targetPlayer;

  protected override void OnStart()
  {
    base.OnStart();
    if (targetPlayer == null)
    {
      int target = Random.Range(0, 2);
      targetPlayer = PlayerRegistry.Players[target];
    }
    homingTime = homingTime + lockTime;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    homingTimer += Time.fixedDeltaTime;
    if (homingTimer <= lockTime)
    {
      return;
    }
    else if (homingTimer <= homingTime)
    {
      Vector2 direction = (Vector2)targetPlayer.position - rb.position;
      direction = direction.normalized;
      float deg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0f, 0f, deg - 90f);

      rb.velocity = direction * speed;
    }
  }

  public void SetTargetPlayer(Transform target)
  {
    targetPlayer = target;
  }
}
