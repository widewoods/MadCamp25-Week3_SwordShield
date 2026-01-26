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
    [SerializeField] private float offscreenMargin = 3f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private bool isRunning=false;
    private Vector2 lockedTargetPos;

    private Action onFinished;
    private Transform Target;
    private Coroutine landRoutine;
    private Collider2D[] cachedColliders;
    private bool[] cachedColliderStates;
    private Renderer[] cachedRenderers;
    private const string JumpLayerName = "TransparentFX";
    private const string DefaultLayerName = "Enemy";

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    public Transform SelectTarget()
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

    public void JumpTarget()
    {
        Target = SelectTarget();
        SetLayerRecursive(JumpLayerName);
        HideTarget();
        CacheAndDisableColliders();

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Vector2 offscreenPos = GetOffscreenAbove(rb.position);
        rb.position = offscreenPos;
    }

    public void LandTarget()
    {
        Debug.Log("landtarget");
        if (Target == null) return;
        ShowTarget();

        if (landRoutine != null)
            StopCoroutine(landRoutine);

        landRoutine = StartCoroutine(LandRoutine(Target.position));
    }

    private IEnumerator LandRoutine(Vector2 targetPos)
    {
        isRunning = true;

        Vector2 start = rb.position;
        float t = 0f;
        float duration = Mathf.Max(0.01f, jumpDuration);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = t * t;
            Vector2 pos = Vector2.Lerp(start, targetPos, eased);
            rb.MovePosition(pos);
            yield return null;
        }

        RestoreColliders();
        isRunning = false;
    }

    private void CacheAndDisableColliders()
    {
        cachedColliders = GetComponentsInChildren<Collider2D>(true);
        cachedColliderStates = new bool[cachedColliders.Length];
        for (int i = 0; i < cachedColliders.Length; i++)
        {
            cachedColliderStates[i] = cachedColliders[i].enabled;
            cachedColliders[i].enabled = false;
        }
    }

    private void RestoreColliders()
    {
        if (cachedColliders == null || cachedColliderStates == null) return;
        for (int i = 0; i < cachedColliders.Length; i++)
            cachedColliders[i].enabled = cachedColliderStates[i];

        SetLayerRecursive(DefaultLayerName);
    }

    public void HideTarget()
    {
        CacheRenderers();
        if (cachedRenderers == null) return;

        for (int i = 0; i < cachedRenderers.Length; i++)
            cachedRenderers[i].enabled = false;
    }

    private void ShowTarget()
    {
        if (cachedRenderers == null) return;

        for (int i = 0; i < cachedRenderers.Length; i++)
            cachedRenderers[i].enabled = true;
    }

    private void CacheRenderers()
    {
        if (cachedRenderers != null) return;
        cachedRenderers = GetComponentsInChildren<Renderer>(true);
    }

    private Vector2 GetOffscreenAbove(Vector2 fromPos)
    {
        Camera cam = Camera.main;
        if (cam == null)
            return fromPos + Vector2.up * (jumpHeight + 5f);

        float z = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, z));
        return new Vector2(fromPos.x, topCenter.y + offscreenMargin);
    }

    private void SetLayerRecursive(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0) return;

        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++)
            transforms[i].gameObject.layer = layer;
    }
}
