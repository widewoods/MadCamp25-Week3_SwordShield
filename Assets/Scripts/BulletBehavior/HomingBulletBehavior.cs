using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBulletBehavior : BulletBehavior
{
  [SerializeField] private float homingTime;
  float homingTimer = 0f;

  private Transform targetPlayer;

  protected override void OnStart()
  {
    base.OnStart();
    int target = Random.Range(0, 2);
    targetPlayer = PlayerRegistry.Players[target];
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    homingTimer += Time.fixedDeltaTime;
    if (homingTimer <= homingTime)
    {
      Vector2 direction = (Vector2)targetPlayer.position - rb.position;
      direction = direction.normalized;
      rb.velocity = direction * speed;
    }
  }
}
