using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossController : MonoBehaviour
{
    // Start is called before the first frame update
    public enum BossState {
        Idle,
        Attack,
        Split,
        Dead
    }

    public BossState CurrentState = BossState.Idle;
    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP;

    // ===== 참조 =====
    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;
    public Transform[] players;

    private SlimeAttack_JumpShot jumpShot;
    private SlimeAttack_BigJump bigJump;
    private SlimeSplitHandler splitHandler;

    // ===== 분열 관련 =====
    [Header("Split")]
    public int splitCount = 0;
    public int maxSplitCount = 2;

    // ===== 내부 제어 =====
    private bool isBusy = false;

    void Awake()
    {
        currentHP = maxHP;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        jumpShot = GetComponent<SlimeAttack_JumpShot>();
        bigJump = GetComponent<SlimeAttack_BigJump>();
        splitHandler = GetComponent<SlimeSplitHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentState != BossState.Idle) return;
        if(CanSplit()) StartSplit();
        else NextAttack();
        return;
    }

    // Change-State Logic
    bool CanSplit(){
        return currentHP <= maxHP * 0.5f && splitCount < maxSplitCount;
    }

    void NextAttack(){
        /*
        float r = Random.value;
        if(r < 0.6f) StartJumpShot();
        else StartBigJump();
        */
        StartJumpShot();
    }

    void ChangeState(BossState st, bool busy){
        CurrentState = st;
        isBusy = busy;
    }

    // In-State Logic
    void StartJumpShot(){
        ChangeState(BossState.Attack, true);
        jumpShot.Execute(OnAttackFinished);
    }

    void StartBigJump(){
        ChangeState(BossState.Attack, true);
        bigJump.Execute(players, OnAttackFinished);
    }

    void StartSplit(){
        ChangeState(BossState.Split, true);
        splitHandler.Execute(OnSplitFinished);
    }

    // Callback Functions
    void OnAttackFinished(){
        ChangeState(BossState.Idle, false);
    }

    void OnSplitFinished(){
        ChangeState(BossState.Idle, false);
    }

    // Playerable Interactions
    public void TakeDamage(float dmg)
    {
        if (CurrentState == BossState.Dead) return;
        currentHP -= dmg;
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        CurrentState = BossState.Dead;
        animator.SetTrigger("Die");
        Destroy(gameObject, 1.5f);
    }
}
