using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRole : MonoBehaviour
{
  [SerializeField] private SwordRole swordRole;

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      swordRole.StartPull();
    }
  }
}
