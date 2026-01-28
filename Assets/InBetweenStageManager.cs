using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InBetweenStageManager : MonoBehaviour
{
  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.RightShift))
    {
      Debug.Log("Load next scene");
    }
    else if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
    {
      Debug.Log("Load next scene");
    }
  }
}
