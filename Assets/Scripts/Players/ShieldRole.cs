using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRole : PlayerHealth
{
  [SerializeField] private float parryDuration;
  [SerializeField] private float parryRadius;
  [SerializeField] private float parryCooldown;

  private float parryTimer = 0f;
  private CircleCollider2D collider;
  private bool isParrying;
  private bool parryReady;
  private float baseColliderSize;

  void Awake()
  {
    collider = GetComponent<CircleCollider2D>();
    baseColliderSize = collider.radius;
  }

  void Update()
  {
    // if (Input.GetKeyDown(KeyCode.LeftShift))
    // {
    //   if (parryReady)
    //   {
    //     StartParry();
    //   }
    // }
    // parryTimer += Time.deltaTime;
    // if (parryTimer >= parryDuration)
    // {
    //   isParrying = false;
    //   collider.radius = baseColliderSize;
    // }

    // if (parryTimer >= parryCooldown)
    // {
    //   parryReady = true;
    // }
  }

  private void StartParry()
  {
    isParrying = true;
    collider.radius = parryRadius;
    parryTimer = 0f;
    parryReady = false;
  }

  private void Parry(Collider2D other)
  {
    BulletBehavior bulletBehavior = other.gameObject.GetComponent<BulletBehavior>();
    bulletBehavior.enabled = false;
    ParriedBehavior parriedBehavior = other.gameObject.AddComponent<ParriedBehavior>();

    Vector3 direction = other.transform.position - transform.position;
    direction = direction.normalized;
    Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
    other.transform.up = direction.normalized;
    rb.velocity = direction * bulletBehavior.Speed;
  }

  protected override void HandleTrigger(Collider2D collision)
  {
    if (isParrying && collision.gameObject.CompareTag("Bullet"))
    {
      Parry(collision);
    }
    if (!isParrying)
    {
      base.HandleTrigger(collision);
    }
  }

}
