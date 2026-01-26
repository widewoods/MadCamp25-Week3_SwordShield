using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry : MonoBehaviour
{
  public static readonly List<Transform> Players = new();

  void OnEnable() => Players.Add(transform);
  void OnDisable() => Players.Remove(transform);
}
