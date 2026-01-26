using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossController : MonoBehaviour
{
    public enum BossState{
        Idle,
        Attack,
        Split,
        Dead
    }
    public BossState currentState = BossState.Idle;
    public BossState CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState == value) return;

            currentState = value;
            //Debug.Log($"[SlimeBoss] State changed [{Time.frameCount}]→ {currentState}", this);
        }
    }

    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP;
    public float AttackDelayTime = 2f;

    // ===== 참조 =====
    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Scripts")]
    private SlimeAttack_JumpShot jumpShot;
    private SlimeAttack_BigJump bigJump;
    private SlimeSplitHandler splitHandler;

    // ===== 내부 제어 =====
    private bool isBusy = false;
    // Start is called before the first frame update
    void Awake()
    {
        // Stats
        currentHP = maxHP;

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
        if(isBusy) return;
        //StartNextAttack();
        Die();
    }

    // Change-State Logic
    void ChangeState(BossState st, bool busy){
        CurrentState = st;
        isBusy = busy;
    }

    // In-State Logic
    void StartNextAttack(){
        float r = Random.value;
        if(r<.6f)StartJumpShot();
        else StartBigJump();
    }

    void StartJumpShot(){
        ChangeState(BossState.Attack, true);
        animator.SetTrigger("SpineAttack");
    }

    void StartBigJump(){
        ChangeState(BossState.Attack, true);
        animator.SetTrigger("Walk");
    }

    void Die()
    {
        ChangeState(BossState.Dead, true);
        animator.SetTrigger("Death");
        // if(splitCount < maxSplitCount)
        //     StartSplit();
    }

    // Callback Functions
    void OnAttackFinished(){
        StartCoroutine(AttackDelay());
    }

    void OnSplitFinished(){
        ChangeState(BossState.Idle, false);
    }

    private IEnumerator AttackDelay(){
        ChangeState(BossState.Idle, true);
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(AttackDelayTime);
        isBusy = false;
    }

}
