using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  [SerializeField] private Rigidbody2D playerRb;
  [SerializeField] private float baseSpeed = 5f;
  private float moveSpeed;
  [SerializeField] private int playerID;

  private Vector2 input;
  private Vector2 externalVelocity;
  private float movementOverrideTimer = 0f;

  public Vector2 Position => playerRb.position;

  void Start()
  {
    moveSpeed = baseSpeed;
  }

  void Update()
  {
    if (playerID == 1)
    {
      input = GetWASDMovement();
    }
    else if (playerID == 2)
    {
      input = GetArrowMovement();
    }
  }
  void FixedUpdate()
  {
    if(movementOverrideTimer > 0f)
    {
      movementOverrideTimer -= Time.fixedDeltaTime;
      if (movementOverrideTimer <= 0f)
      {
        moveSpeed = baseSpeed;
      }
    }
    
    Vector2 vInput = input * moveSpeed;
    Vector2 finalVelocity = vInput + externalVelocity;

    playerRb.MovePosition(Position + finalVelocity * Time.fixedDeltaTime);
  }

  public void AddExternalVelocity(Vector2 velocity)
  {
    externalVelocity += velocity;
  }

  public void SetExternalVelocity(Vector2 velocity)
  {
    externalVelocity = velocity;
  }

  public void SetMovementOverride(float speedFactor, float duration)
  {
    moveSpeed *= speedFactor;
    movementOverrideTimer = duration;
  }

  private Vector2 GetWASDMovement()
  {
    Vector2 input = Vector2.zero;
    if (Input.GetKey(KeyCode.W))
    {
      input.y += 1;
    }
    if (Input.GetKey(KeyCode.A))
    {
      input.x -= 1;
    }
    if (Input.GetKey(KeyCode.S))
    {
      input.y -= 1;
    }
    if (Input.GetKey(KeyCode.D))
    {
      input.x += 1;
    }
    return input.normalized;
  }

  private Vector2 GetArrowMovement()
  {
    Vector2 input = Vector2.zero;
    if (Input.GetKey(KeyCode.UpArrow))
    {
      input.y += 1;
    }
    if (Input.GetKey(KeyCode.LeftArrow))
    {
      input.x -= 1;
    }
    if (Input.GetKey(KeyCode.DownArrow))
    {
      input.y -= 1;
    }
    if (Input.GetKey(KeyCode.RightArrow))
    {
      input.x += 1;
    }
    return input.normalized;
  }
}