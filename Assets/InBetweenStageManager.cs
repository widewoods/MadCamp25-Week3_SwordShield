using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InBetweenStageManager : MonoBehaviour
{
  public static Action OnReady;
  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.RightShift))
    {
      OnReady?.Invoke();
    }
    else if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
    {
      OnReady?.Invoke();
    }
  }
}
