using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
  private enum BossState { Idle, Chasing, Attack_Shockwave, Attack_Missile, Attack_Frenzy, Attack_Sword, Attack_Circle, Stunned, Dead }
  private BossState currentState = BossState.Idle;

  [Header("References")]
  [SerializeField] private Animator animator;
  [SerializeField] private Transform spriteTransform;
  [SerializeField] private Transform centerTransform;
  [SerializeField] private Rigidbody2D rb;
  [SerializeField] private SpriteRenderer spriteRenderer;


  [Header("Move")]
  [SerializeField] private float moveSpeed = 3f;
  [SerializeField] private float idleTime = 1f;
  [SerializeField] private float stunDuration = 1f;

  [Header("Homing Attack")]
  [SerializeField] private SpawnArc spawnArc;
  [SerializeField] private GameObject homingBulletPrefab;
  [SerializeField] private int homingMissileCount;
  [SerializeField] private Transform[] missileSpawners;

  [Header("Shockwave Attack")]
  [SerializeField] private SpawnSimple spawnSimple;
  [SerializeField] private GameObject shockwaveBulletPrefab;

  [Header("Frenzy Attack")]
  [SerializeField] private SpawnRing spawnRing;
  [SerializeField] private GameObject frenzyBulletPrefab;

  [Header("Audio")]
  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioClip burstSound;
  [SerializeField] private AudioClip magicMissileFire;
  [SerializeField] private AudioClip frenzyFire;
  [SerializeField] private AudioClip ShockwaveFire;

  private bool attackStarted;

  private bool enteredPhase2;
  private bool facingLeft;
  private Coroutine stateCoroutine;
  private Transform target;

  private float soundTime = 0.3f;
  private float soundTimer = 0f;

  void Start()
  {
    target = PlayerRegistry.Players[0];
    stateCoroutine = StartCoroutine(StateLoop(currentState));
  }

  void OnEnable()
  {
    BurstBulletBehavior.OnBurst += PlaySoundOnBurst;
  }

  void OnDisable()
  {
    BurstBulletBehavior.OnBurst -= PlaySoundOnBurst;
  }

  void Update()
  {
    soundTimer += Time.deltaTime;
  }

  IEnumerator StateLoop(BossState s)
  {
    switch (s)
    {
      case BossState.Idle:
        yield return new WaitForSeconds(idleTime);
        ChooseNextState();
        break;
      case BossState.Stunned:
        yield return StunRoutine(stunDuration);
        ChooseNextState();
        break;
      case BossState.Chasing:
        yield return Chase();
        ChooseNextState();
        break;
      case BossState.Attack_Shockwave:
        yield return ShockwaveAttack();
        ChooseNextState();
        break;
      case BossState.Attack_Frenzy:
        yield return FrenzyAttack();
        ChooseNextState();
        break;
      case BossState.Attack_Missile:
        yield return MissileAttack();
        ChooseNextState();
        break;
    }
  }

  void SwitchState(BossState next)
  {
    if (currentState == BossState.Dead) return;

    if (stateCoroutine != null) StopCoroutine(stateCoroutine);

    currentState = next;
    stateCoroutine = StartCoroutine(StateLoop(currentState));
  }

  int attackPattern = 0;
  private void ChooseNextState()
  {
    if (currentState == BossState.Stunned)
    {
      SwitchState(BossState.Idle);
    }
    else if (currentState == BossState.Idle)
    {
      SwitchState(BossState.Chasing);
    }
    else if (currentState == BossState.Chasing)
    {
      int randomAttack = UnityEngine.Random.Range(0, 5);
      if (enteredPhase2)
      {
        attackPattern++;
      }
      if (attackPattern == 5)
      {
        SwitchState(BossState.Attack_Frenzy);
        attackPattern = 0;
      }
      else if (randomAttack <= 1)
      {
        SwitchState(BossState.Attack_Missile);
      }
      else if (randomAttack <= 4)
      {
        SwitchState(BossState.Attack_Shockwave);
      }

    }
    else if (currentState == BossState.Attack_Missile)
    {
      SwitchState(BossState.Idle);
    }
    else if (currentState == BossState.Attack_Shockwave)
    {
      SwitchState(BossState.Idle);
    }
    else if (currentState == BossState.Attack_Frenzy)
    {
      SwitchState(BossState.Idle);
    }
  }

  IEnumerator MissileAttack()
  {
    attackStarted = false;

    audioSource.PlayOneShot(magicMissileFire);
    animator.SetTrigger("Missile");

    yield return new WaitUntil(() => attackStarted);

    Vector2 direction = target.position - spriteTransform.position;
    direction = direction.normalized;
    spawnArc.bulletPrefab = homingBulletPrefab;

    Vector3 spawnPosition = spriteTransform.position;
    float spawnAngle = 90f;
    if (facingLeft)
    {
      spawnPosition.x -= 1;
      spawnAngle = -90f;
    }
    yield return spawnArc.Spawn(homingMissileCount, spawnAngle, spawnPosition, homingMissileCount * 20);
    if (enteredPhase2)
    {
      for (int i = 0; i < missileSpawners.Length; i++)
      {
        yield return spawnArc.Spawn(3, spawnAngle, missileSpawners[i].position, 90);
      }
    }
  }

  IEnumerator ShockwaveAttack()
  {
    attackStarted = false;

    audioSource.PlayOneShot(ShockwaveFire);
    animator.SetTrigger("Shockwave");

    yield return new WaitUntil(() => attackStarted);

    Vector2 direction = target.position - spriteTransform.position;
    direction = direction.normalized;
    spawnSimple.bulletPrefab = shockwaveBulletPrefab;

    Vector3 spawnPosition = spriteTransform.position;
    if (facingLeft)
    {
      spawnPosition.x -= 1;
    }
    yield return spawnSimple.Spawn(1, spawnPosition, direction, 0.3f);
  }

  IEnumerator FrenzyAttack()
  {
    attackStarted = false;

    animator.SetTrigger("Frenzy");

    yield return new WaitUntil(() => attackStarted);
    for (int i = 0; i < 4; i++)
    {
      audioSource.PlayOneShot(frenzyFire);
      yield return spawnRing.Spawn(10, 2f, centerTransform.position);
      yield return new WaitForSeconds(0.2f);
      audioSource.PlayOneShot(frenzyFire);
      yield return spawnRing.Spawn(10, 2f, centerTransform.position, 360f / 10 / 3);
      yield return new WaitForSeconds(0.2f);
      audioSource.PlayOneShot(frenzyFire);
      yield return spawnRing.Spawn(10, 2f, centerTransform.position, 360f / 10 / 3 * 2);
      yield return new WaitForSeconds(0.2f);
    }
  }

  IEnumerator Chase()
  {
    float chaseDuration = 2f;
    float chaseTimer = 0f;

    int randomTarget = UnityEngine.Random.Range(0, PlayerRegistry.Players.Count);
    target = PlayerRegistry.Players[randomTarget];

    animator.SetTrigger("Walk");
    while (chaseTimer < chaseDuration)
    {
      chaseTimer += Time.fixedDeltaTime;

      Vector3 targetPosition = target.position;
      Vector3 direction = (targetPosition - transform.position).normalized;
      rb.MovePosition((Vector3)rb.position + direction * moveSpeed * Time.fixedDeltaTime);


      if (direction.x <= 0)
      {
        spriteRenderer.flipX = true;
        facingLeft = true;
      }
      else
      {
        spriteRenderer.flipX = false;
        facingLeft = false;
      }

      yield return new WaitForFixedUpdate();
    }

    rb.velocity = Vector2.zero;
  }

  public void Stun()
  {
    SwitchState(BossState.Stunned);
  }

  public void Die()
  {
    animator.SetTrigger("Dead");
    SwitchState(BossState.Dead);
  }

  IEnumerator StunRoutine(float duration)
  {
    if (rb) rb.velocity = Vector2.zero;

    if (spriteRenderer) spriteRenderer.color = new Color(0.3f, 0.3f, 1f, 1f);

    animator.Play("Mage_Idle", 0, 0f);

    yield return new WaitForSeconds(duration);

    if (spriteRenderer) spriteRenderer.color = Color.white;
  }

  public void AnimEvent_AttackStart() => attackStarted = true;
  public void EnterPhase2()
  {
    enteredPhase2 = true;
    homingMissileCount *= 2;
  }

  private void PlaySoundOnBurst()
  {
    if (soundTimer >= soundTime)
    {
      audioSource.PlayOneShot(burstSound);
      soundTimer = 0;
    }
  }
}
