using UnityEngine;
using System.Collections;
using System;

public class SlimeAttack_SuperDash : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float dashDuration = 120.0f;

    [Header("Collision")]
    [SerializeField] private LayerMask wallLayer;

    [Header("Rolling")]
    [SerializeField] private float rollFactor = 120f; // 속도 -> 회전 변환 계수 (조절)

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

        Direction = UnityEngine.Random.insideUnitCircle.normalized;
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision");
        if (!isDashing) return;
        if (((1 << col.gameObject.layer) & wallLayer) == 0) return;

        Vector2 normal = col.GetContact(0).normal;
        Direction = Vector2.Reflect(Direction, normal);

        // ✅ 속도 변화 없이 방향만 반사
        rb.velocity = Direction * dashSpeed;
        ApplyRollFromVelocity(rb.velocity);
    }

    private void ApplyRollFromVelocity(Vector2 v)
    {
        // 2D에서 "굴러가는" 느낌: 이동 방향(x 부호)에 따라 회전 방향 결정
        float sign = Mathf.Sign(Mathf.Abs(v.x) < 0.0001f ? 1f : v.x);
        rb.angularVelocity = -sign * v.magnitude * rollFactor;
    }
}
