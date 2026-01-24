using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GolemController : MonoBehaviour
{
  private enum BossState { Idle, Chasing, Attacking_Strike, Attacking_Laser, Spawn_Towers, Invincible, Dead }
  private BossState currentState = BossState.Idle;

  [Header("References")]
  [SerializeField] private Transform[] playerTransforms;
  [SerializeField] private Rigidbody2D rb;

  [Header("Move")]
  [SerializeField] private float moveSpeed = 3f;
  [SerializeField] private float idleTime = 1f;

  [Header("Attacks")]
  [SerializeField] private SpawnRing spawnRingAttack;
  [SerializeField] private SpawnMinion spawnMinion;

  private Coroutine stateCoroutine;
  private bool protectionBroken = false;

  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(StateLoop(currentState));
  }

  // Update is called once per frame
  void Update()
  {

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
        yield return StartCoroutine(ChasingState());
        ChooseNextState();
        break;
      case BossState.Attacking_Strike:
        yield return StartCoroutine(spawnRingAttack.Spawn(12, 2f, transform.position));
        SwitchState(BossState.Idle);
        break;
      case BossState.Spawn_Towers:
        yield return StartCoroutine(TowerState());
        break;
      case BossState.Invincible:
        yield return new WaitUntil(() => protectionBroken);
        protectionBroken = false;
        SwitchState(BossState.Idle);
        break;
    }
  }

  void ChooseNextState()
  {
    if (currentState == BossState.Idle)
    {
      SwitchState(BossState.Chasing);
    }
    if (currentState == BossState.Chasing)
    {
      int attackChoice = Random.Range(0, 2);
      if (attackChoice == 0)
      {
        SwitchState(BossState.Attacking_Strike);
      }
      else
      {
        SwitchState(BossState.Spawn_Towers);
      }
    }
  }

  IEnumerator ChasingState()
  {
    float chaseDuration = 3f;
    float chaseTimer = 0f;

    Transform targetTransform = playerTransforms[Random.Range(0, 2)];

    while (chaseTimer < chaseDuration)
    {
      chaseTimer += Time.deltaTime;

      Vector3 targetPosition = targetTransform.position;
      Vector3 direction = (targetPosition - transform.position).normalized;
      rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

      yield return null;
    }

    rb.velocity = Vector2.zero;
  }

  IEnumerator TowerState()
  {
    for (int x = -2; x <= 2; x += 4)
    {
      for (int y = -2; y <= 2; y += 4)
      {
        Vector3 spawnPos = transform.position + new Vector3(x * 4f, y * 2f, 0f);
        spawnMinion.Spawn(spawnPos);
      }
    }
    SwitchState(BossState.Invincible);
    yield return null;
  }

  public void BreakProtection()
  {
    protectionBroken = true;
  }
}
