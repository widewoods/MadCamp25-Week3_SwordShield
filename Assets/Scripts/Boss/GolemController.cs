using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GolemController : MonoBehaviour
{
  private enum BossState { Idle, Chasing, Attacking_Strike, Attacking_Laser, Spawn_Towers, Dead }
  private BossState currentState = BossState.Idle;

  [Header("References")]
  [SerializeField] private Transform[] playerTransforms;
  [SerializeField] private Rigidbody2D rb;
  [SerializeField] private Transform[] towerSpawnPoints;
  [SerializeField] private Animator animator;


  [Header("Audio")]
  [SerializeField] private AudioClip strikeSound;
  [SerializeField] private AudioClip humSound;
  [SerializeField] private AudioSource audioSource;


  [Header("Move")]
  [SerializeField] private float moveSpeed = 3f;
  [SerializeField] private float idleTime = 1f;

  [Header("Attacks")]
  [SerializeField] private SpawnRing spawnRingAttack;
  [SerializeField] private SpawnMinion spawnMinion;

  private Coroutine stateCoroutine;
  private bool protectionBroken = true;
  private bool enteredPhase2 = false;

  public event Action OnBossDeath;
  public bool ProtectionBroken => protectionBroken;

  void OnEnable()
  {
    TowerHealth.OnTowersBroken += BreakProtection;
  }

  // Start is called before the first frame update
  void Start()
  {
    stateCoroutine = StartCoroutine(StateLoop(currentState));
    audioSource = GetComponent<AudioSource>();
  }
  void OnDisable()
  {
    TowerHealth.OnTowersBroken -= BreakProtection;
  }

  void SwitchState(BossState next)
  {
    if (currentState == BossState.Dead) return;

    if (stateCoroutine != null) StopCoroutine(stateCoroutine);

    currentState = next;
    stateCoroutine = StartCoroutine(StateLoop(currentState));
  }

  IEnumerator StateLoop(BossState s)
  {
    switch (s)
    {
      case BossState.Idle:
        yield return new WaitForSeconds(idleTime);
        ChooseNextState();
        break;
      case BossState.Chasing:
        yield return ChasingState();
        ChooseNextState();
        break;
      case BossState.Attacking_Strike:
        yield return AttackRing();
        ChooseNextState();
        break;
      case BossState.Spawn_Towers:
        protectionBroken = false;
        yield return SpawnTowersState();
        yield return new WaitUntil(() => protectionBroken);
        ChooseNextState();
        break;
    }
  }

  void ChooseNextState()
  {
    if (enteredPhase2)
    {
      SwitchState(BossState.Spawn_Towers);
      return;
    }

    if (currentState == BossState.Idle)
    {
      audioSource.Stop();
      audioSource.clip = humSound;
      audioSource.Play();
      SwitchState(BossState.Chasing);
    }
    else if (currentState == BossState.Chasing)
    {
      int attackChoice = UnityEngine.Random.Range(0, 1);
      if (attackChoice == 0)
      {
        SwitchState(BossState.Attacking_Strike);
      }

    }
    else if (currentState == BossState.Attacking_Strike)
    {
      SwitchState(BossState.Idle);
    }
    else if (currentState == BossState.Spawn_Towers)
    {
      SwitchState(BossState.Idle);
    }
  }

  IEnumerator ChasingState()
  {
    float chaseDuration = 1.5f;
    float chaseTimer = 0f;

    Transform targetTransform = playerTransforms[UnityEngine.Random.Range(0, 2)];

    while (chaseTimer < chaseDuration)
    {
      chaseTimer += Time.fixedDeltaTime;

      Vector3 targetPosition = targetTransform.position;
      Vector3 direction = (targetPosition - transform.position).normalized;
      rb.MovePosition((Vector3)rb.position + direction * moveSpeed * Time.fixedDeltaTime);

      yield return new WaitForFixedUpdate();
    }

    rb.velocity = Vector2.zero;
  }

  IEnumerator SpawnTowersState()
  {
    animator.SetBool("isProtected", true);
    for (int i = 0; i < towerSpawnPoints.Length; i++)
    {
      spawnMinion.Spawn(towerSpawnPoints[i].position);
      yield return new WaitForSeconds(0.5f);
    }
  }

  IEnumerator AttackRing()
  {
    animator.SetTrigger("Strike");
    yield return new WaitForSeconds(0.7f);
    FindObjectOfType<CameraShake>().Shake(0.3f, 0.6f);
    audioSource.Stop();
    audioSource.clip = strikeSound;
    audioSource.Play();
    yield return spawnRingAttack.Spawn(12, 2f, transform.position);
    yield return new WaitForSeconds(0.5f);
    yield return spawnRingAttack.Spawn(12, 1f, transform.position, 15f);
    animator.ResetTrigger("Strike");
  }

  public void Phase2()
  {
    enteredPhase2 = true;
  }

  public void BreakProtection()
  {
    animator.SetBool("isProtected", false);
    protectionBroken = true;
  }

  public void Die()
  {
    GetComponentInChildren<Light2D>().enabled = false;
    StopCoroutine(stateCoroutine);
    animator.SetBool("isDead", true);
    currentState = BossState.Dead;
    OnBossDeath?.Invoke();
  }
}
