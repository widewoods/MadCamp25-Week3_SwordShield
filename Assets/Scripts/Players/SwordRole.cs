using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRole : MonoBehaviour
{
  private enum PullState { Idle, StartUp, Pulling }


  [Header("References")]
  [SerializeField] private Rigidbody2D swordRb;
  [SerializeField] private PlayerController swordPlayerController;
  [SerializeField] private PlayerController shieldPlayerController;
  [SerializeField] private ShowSlashTrail slashTrail;
  [SerializeField] private Transform visualPivot;
  [SerializeField] private PlayerHealth health;

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
  private float SPRITE_ROTATION_OFFSET = -90f;

  void Start()
  {
    swordRb = GetComponent<Rigidbody2D>();
    swordPlayerController = GetComponent<PlayerController>();
    slashTrail = GetComponentInChildren<ShowSlashTrail>();
  }

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
    else
    {
      FaceDown();
    }
  }

  public void StartPull()
  {
    if (currentState == PullState.Idle)
    {
      health.SetInvincible(true);
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
    Vector2 targetPos = shieldPlayerController.Position;
    Vector2 playerPos = swordPlayerController.Position;
    Vector2 toTarget = targetPos - playerPos;
    Vector2 pullDirection = toTarget.normalized;
    float distance = toTarget.magnitude;

    if (distance <= stopDistance)
    {
      currentState = PullState.Idle;
      swordPlayerController.SetExternalVelocity(Vector2.zero);
      slashTrail.EndTrail();
      health.SetInvincible(false);
      return;
    }

    if (currentState == PullState.StartUp)
    {
      startUpTimer += Time.fixedDeltaTime;
      if (startUpTimer >= startUpDuration)
      {
        slashTrail.StartTrail();
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
      swordPlayerController.SetExternalVelocity(pullDirection * maxPullStrength * accelerationCurve.Evaluate(u));
    }
  }

  private void FaceShield()
  {
    Vector2 toShield = swordPlayerController.Position - shieldPlayerController.Position;

    float targetDeg = Mathf.Atan2(toShield.y, toShield.x) * Mathf.Rad2Deg + SPRITE_ROTATION_OFFSET;
    float current = visualPivot.eulerAngles.z;
    float next = Mathf.LerpAngle(current, targetDeg, 1f - Mathf.Exp(-12f * Time.deltaTime));

    visualPivot.rotation = Quaternion.Euler(0, 0, next);
  }

  private void FaceDown()
  {
    Vector2 down = Vector2.down;
    float targetDeg = Mathf.Atan2(down.y, down.x) * Mathf.Rad2Deg + 90f;
    float current = visualPivot.eulerAngles.z;
    float next = Mathf.LerpAngle(current, targetDeg, 1f - Mathf.Exp(-12f * Time.deltaTime));

    visualPivot.rotation = Quaternion.Euler(0, 0, next);
  }
}
