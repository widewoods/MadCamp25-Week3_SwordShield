using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PullAbility : MonoBehaviour
{
  private enum PullState { Idle, StartUp, Pulling }


  [SerializeField] private Rigidbody2D swordRb;
  [SerializeField] private PlayerController swordPlayerController;

  [Header("Pull")]
  [SerializeField] private float maxPullStrength = 5f;
  [SerializeField] private float stopDistance = 1f;
  [SerializeField] private float pullSlowdownFactor = 0.3f;
  [SerializeField] private float accelerationTime = 1f;
  [SerializeField] private float startUpDuration = 0.5f;
  [SerializeField] private AnimationCurve accelerationCurve;

  private PullState currentState = PullState.Idle;

  private float pullTimer = 0f;
  private float startUpTimer = 0f;
  private float SPRITE_ROTATION_OFFSET = 135f;

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      StartPull();
    }
  }

  void FixedUpdate()
  {
    if (currentState != PullState.Idle)
    {
      Pull();
    }
  }

  private void StartPull()
  {
    if (currentState == PullState.Idle)
    {
      pullTimer = 0f;
      startUpTimer = 0f;
      currentState = PullState.StartUp;
      swordPlayerController.SetMovementOverride(pullSlowdownFactor, accelerationTime);
    }
    else
    {
      return;
    }
  }

  private void Pull()
  {
    Vector2 targetPos = swordPlayerController.Position;
    Vector2 playerPos = (Vector2)transform.position;
    Vector2 toTarget = targetPos - playerPos;
    Vector2 pullDirection = toTarget.normalized;
    float distance = toTarget.magnitude;

    if (distance <= stopDistance)
    {
      currentState = PullState.Idle;
      swordPlayerController.SetExternalVelocity(Vector2.zero);
      return;
    }

    if (currentState == PullState.StartUp)
    {
      startUpTimer += Time.fixedDeltaTime;
      if (startUpTimer >= startUpDuration)
      {
        currentState = PullState.Pulling;
      }
      FaceShield();
      swordPlayerController.SetExternalVelocity(Vector2.zero);
      return;
    }
    else if (currentState == PullState.Pulling)
    {
      pullTimer += Time.fixedDeltaTime;
      FaceShield();
      float u = Mathf.Min(1, pullTimer / accelerationTime);
      swordPlayerController.SetExternalVelocity(-pullDirection * maxPullStrength * accelerationCurve.Evaluate(u));
    }
  }

  private void FaceShield()
  {
    Vector2 toShield = swordPlayerController.Position - (Vector2)transform.position;

    float targetDeg = Mathf.Atan2(toShield.y, toShield.x) * Mathf.Rad2Deg + SPRITE_ROTATION_OFFSET;
    float newDeg = Mathf.MoveTowardsAngle(swordRb.rotation, targetDeg, 360f * Time.fixedDeltaTime);
    swordRb.MoveRotation(newDeg);
  }
}
