using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GolemController : MonoBehaviour
{
  private enum BossState { Idle, Chasing, Attacking_Strike, Attacking_Laser, Protecting, Dead }
  private BossState currentState = BossState.Idle;

  [Header("References")]
  [SerializeField] private Transform[] playerTransforms;
  [SerializeField] private Rigidbody2D rb;

  [Header("Move")]
  [SerializeField] private float moveSpeed = 3f;
  [SerializeField] private float idleTime = 1f;

  [Header("Attacks")]
  [SerializeField] private SpawnRing spawnRingAttack;

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
    if (next == BossState.Dead) return;

    currentState = next;
    StartCoroutine(StateLoop(currentState));
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
        break;
      case BossState.Attacking_Strike:
        yield return StartCoroutine(spawnRingAttack.Spawn(12, 2f, transform.position));
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
    SwitchState(BossState.Attacking_Strike);
  }
}
