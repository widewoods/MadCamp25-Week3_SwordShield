using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField]
  private Rigidbody2D playerRb;
  public float moveSpeed = 5f;

  [SerializeField]
  private int playerID;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void FixedUpdate()
  {
    if (playerID == 1)
    {
      Vector2 input = GetWASDMovement();
      playerRb.MovePosition(playerRb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
    else if (playerID == 2)
    {
      Vector2 input = GetArrowMovement();
      playerRb.MovePosition(playerRb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
  }

  Vector2 GetWASDMovement()
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

  Vector2 GetArrowMovement()
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