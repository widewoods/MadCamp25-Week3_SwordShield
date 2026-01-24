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

    [Header("Offscreen Jump")]
    [SerializeField] private float offscreenMargin = 1f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private bool isRunning=false;
    private Vector2 lockedTargetPos;

    private Action onFinished;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    public void Execute(Action onFinished)
    {
        Debug.Log("Bigjump-execute");
        if (isRunning) return;

        Transform target = SelectTarget();

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

        var colliders = GetComponentsInChildren<Collider2D>(true);
        var colliderStates = new bool[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            colliderStates[i] = colliders[i].enabled;
            colliders[i].enabled = false;
        }

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Vector2 offscreenPos = GetOffscreenAbove(lockedTargetPos);
        rb.position = offscreenPos;

        float t = 0f;
        float duration = Mathf.Max(0.01f, jumpDuration);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            float eased = t * t;
            Vector2 pos = Vector2.Lerp(offscreenPos, lockedTargetPos, eased);

            rb.MovePosition(pos);
            yield return null;
        }

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = colliderStates[i];

        animator?.SetTrigger("Land");
        yield return new WaitForSeconds(postLandDelay);

        isRunning = false;
        onFinished?.Invoke();
    }

    private Vector2 GetOffscreenAbove(Vector2 targetPos)
    {
        Camera cam = Camera.main;
        if (cam == null)
            return targetPos + Vector2.up * (jumpHeight + 5f);

        float z = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, z));
        return new Vector2(targetPos.x, topCenter.y + offscreenMargin);
    }

    private Transform SelectTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        if (players == null || players.Length == 0)
            return null;

        var alive = new System.Collections.Generic.List<Transform>();

        foreach (var p in players)
        {
            if (p != null && p.activeInHierarchy)
                alive.Add(p.transform);
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
