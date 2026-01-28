using UnityEngine;
using System.Collections;
using System;

public class SlimeAttack_SuperDash : MonoBehaviour
{
  [Header("Refs")]
  [SerializeField] private Rigidbody2D rb;
  [SerializeField] private Animator animator;
  [SerializeField] private Collider2D myCol;

  [Header("Dash")]
  [SerializeField] private float dashSpeed = 8f;
  [SerializeField] private float dashDuration = 120.0f;

  [Header("Collision")]
  [SerializeField] private LayerMask wallLayer;

  [Header("Rolling")]
  [SerializeField] private float rollFactor = 120f;

  private bool isDashing;
  private Coroutine stopCo;
  private Vector2 Direction;

  void Awake()
  {
    if (rb == null) rb = GetComponent<Rigidbody2D>();
    if (animator == null) animator = GetComponentInChildren<Animator>(); // Animator가 자식(Visual)에 있을 때 대비
  }

  public void StartRicochetRandom(Action OnFinished)
  {
    if (isDashing) return;

    isDashing = true;

    int targetIndex = UnityEngine.Random.Range(0, PlayerRegistry.Players.Count);
    Transform target = PlayerRegistry.Players[targetIndex];
    Direction = transform.position - target.position;
    Direction = Direction.normalized;

    rb.velocity = Direction * dashSpeed;
    ApplyRollFromVelocity(rb.velocity);

    if (stopCo != null) StopCoroutine(stopCo);
    stopCo = StartCoroutine(StopAfterTime(OnFinished));
  }

  private IEnumerator StopAfterTime(Action OnFinished)
  {
    yield return new WaitForSeconds(dashDuration);
    StopRicochet(OnFinished);
  }

  private void StopRicochet(Action OnFinished)
  {
    isDashing = false;
    rb.velocity = Vector2.zero;
    rb.angularVelocity = 0f;
    OnFinished?.Invoke();
  }

  void OnTriggerStay2D(Collider2D col)
  {
    if (!isDashing) return;
    if (((1 << col.gameObject.layer) & wallLayer.value) == 0) return;


    ColliderDistance2D d = myCol.Distance(col);
    Vector2 normal = d.normal;


    if (normal.sqrMagnitude < 1e-6f) return;

    int targetIndex = UnityEngine.Random.Range(0, PlayerRegistry.Players.Count);
    Transform target = PlayerRegistry.Players[targetIndex];
    Direction = transform.position - target.position;
    Direction = Direction.normalized;

    rb.velocity = -Direction * dashSpeed;
    ApplyRollFromVelocity(rb.velocity);
  }

  private void ApplyRollFromVelocity(Vector2 v)
  {
    float sign = Mathf.Sign(Mathf.Abs(v.x) < 0.0001f ? 1f : v.x);
    rb.angularVelocity = -sign * v.magnitude * rollFactor;
  }
}
