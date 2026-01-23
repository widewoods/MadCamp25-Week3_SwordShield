using System;
using System.Collections;
using UnityEngine;

public class SlimeAttack_JumpShot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 8f;
    [SerializeField] private float postLandDelay = 0.3f;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 12;
    [SerializeField] private float bulletSpeed = 7f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.08f;

    private bool isRunning = false;

    public void Execute(Action onFinished)
    {
        if (isRunning) return;
        StartCoroutine(Routine(onFinished));
    }

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    private IEnumerator Routine(Action onFinished)
    {
        isRunning = true;

        // 1️⃣ 점프
        animator?.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        // 2️⃣ 착지 대기
        yield return new WaitUntil(IsGrounded);

        // 3️⃣ 착지 연출 + 탄막 발사
        animator?.SetTrigger("Land");
        ShootRadial();

        // 4️⃣ 착지 후 딜레이
        yield return new WaitForSeconds(postLandDelay);

        isRunning = false;
        onFinished?.Invoke();
    }

    private void ShootRadial()
    {
        if (bulletPrefab == null) return;

        Vector2 origin = rb.position;
        float step = 360f / Mathf.Max(1, bulletCount);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * step;
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ).normalized;

            GameObject b = Instantiate(bulletPrefab, origin, Quaternion.identity);

            if (b.TryGetComponent<SimpleBullet2D>(out var bullet))
            {
                bullet.Launch(dir, bulletSpeed);
            }
            else
            {
                var brb = b.GetComponent<Rigidbody2D>();
                if (brb != null) brb.velocity = dir * bulletSpeed;
            }
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
            return rb.velocity.y <= 0.01f;

        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundMask
        ) != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
