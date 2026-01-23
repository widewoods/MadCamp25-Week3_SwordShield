using System;
using System.Collections;
using UnityEngine;

public class SlimeAttack_BigJump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private float jumpDuration = 0.6f;
    [SerializeField] private float postLandDelay = 0.4f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private bool isRunning;
    private Vector2 lockedTargetPos;

    private Action onFinished;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    public void Execute(Transform[] players, Action onFinished)
    {
        if (isRunning) return;

        Transform target = SelectTarget(players);
        if (target == null)
        {
            onFinished?.Invoke();
            return;
        }

        this.onFinished = onFinished;
        lockedTargetPos = target.position;

        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        isRunning = true;

        animator?.SetTrigger("Charge");
        yield return new WaitForSeconds(0.3f); // 텔레그래프

        animator?.SetTrigger("Jump");

        Vector2 start = rb.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / jumpDuration;

            float height = 4f * jumpHeight * t * (1 - t); // 포물선
            Vector2 pos = Vector2.Lerp(start, lockedTargetPos, t);
            pos.y += height;

            rb.MovePosition(pos);
            yield return null;
        }

        // 착지 대기
        yield return new WaitUntil(IsGrounded);

        animator?.SetTrigger("Land");
        yield return new WaitForSeconds(postLandDelay);

        isRunning = false;
        onFinished?.Invoke();
    }

    private Transform SelectTarget(Transform[] players)
    {
        if (players == null || players.Length == 0)
            return null;

        // 살아있는 플레이어만 수집
        var alive = new System.Collections.Generic.List<Transform>();
        foreach (var p in players)
        {
            if (p != null && p.gameObject.activeInHierarchy)
                alive.Add(p);
        }

        if (alive.Count == 0)
            return null;

        int idx = UnityEngine.Random.Range(0, alive.Count);
        return alive[idx];
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
