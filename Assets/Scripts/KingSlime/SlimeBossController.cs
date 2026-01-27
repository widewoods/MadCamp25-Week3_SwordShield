using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossController : MonoBehaviour
{
  public enum BossState
  {
    Idle,
    Attack,
    Split,
    Dead
  }
  private BossState currentState = BossState.Idle;
  public BossState CurrentState
  {
    get => currentState;
    private set
    {
      if (currentState == value) return;

      currentState = value;
    }
  }

  public bool isInvincible = false;

  [Header("Stats")]
  public float AttackDelayTime = 2f;
  [SerializeField] private float moveSpeed = 5f;

  // ===== 참조 =====
  [Header("References")]
  public Rigidbody2D rb;
  public Animator animator;
  [SerializeField] private CircleCollider2D circleCollider;
  [SerializeField] private Transform physicsTransform;



  [Header("Scripts")]
  private SlimeAttack_JumpShot jumpShot;
  private SlimeAttack_BigJump bigJump;
  private SlimeSplitHandler splitHandler;

  // ===== 내부 제어 =====
  private bool isBusy = false;
  // Start is called before the first frame update
  void Awake()
  {
    // Reference 
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();

    // Scripts
    jumpShot = GetComponent<SlimeAttack_JumpShot>();
    bigJump = GetComponent<SlimeAttack_BigJump>();
    splitHandler = GetComponent<SlimeSplitHandler>();
  }

  // Update is called once per frame
  void Update()
  {
    if (isBusy) return;
    StartNextAttack();
  }

  // Change-State Logic
  void ChangeState(BossState st, bool busy)
  {
    CurrentState = st;
    isBusy = busy;
  }

  // In-State Logic
  void StartNextAttack()
  {
    float r = Random.value;
    if (r < .6f) StartJumpShot();
    else StartBigJump();
  }

  void StartJumpShot()
  {
    ChangeState(BossState.Attack, true);
    animator.SetTrigger("SpineAttack");
  }

  void StartBigJump()
  {
    ChangeState(BossState.Attack, true);
    animator.SetTrigger("Walk");
  }

  public void Die()
  {
    StopAllCoroutines();
    ChangeState(BossState.Dead, true);
    animator.SetTrigger("Death");
    enabled = false;
    circleCollider.enabled = false;
    for (int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
  }

  // Callback Functions
  void OnAttackFinished()
  {
    StartCoroutine(AttackDelay());
  }

  void OnSplitFinished()
  {
    ChangeState(BossState.Idle, false);
  }

  private IEnumerator AttackDelay()
  {
    ChangeState(BossState.Idle, true);
    animator.SetTrigger("Idle");
    Transform target = PlayerRegistry.Players[0];
    float t = 0f;
    while (t < AttackDelayTime)
    {
      Vector2 direction = (Vector2)target.transform.position - (Vector2)physicsTransform.position;
      direction = direction.normalized;
      rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime * (Mathf.Sin(t * Mathf.PI * 2) + 1));
      t += Time.fixedDeltaTime;
      yield return new WaitForFixedUpdate();
    }

    isBusy = false;
  }

}
