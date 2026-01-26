using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SineBulletBehavior : BulletBehavior
{
  [SerializeField] private float frequency;
  [SerializeField] private float amplitude;

  Vector2 forward;
  Vector2 right;
  Vector2 startPos;
  float t0;

  protected override void OnStart()
  {
    base.OnStart();
    forward = initialDirection;
    right = new Vector2(-forward.y, forward.x);
    t0 = Time.time;
    startPos = transform.position;
  }

  void FixedUpdate()
  {
    float t = Time.time - t0;
    Vector2 basePos = startPos + forward * speed * t;
    float offset = Mathf.Sin(t * frequency * Mathf.PI * 2f) * amplitude;

    rb.MovePosition(basePos + right * offset);
  }
}
