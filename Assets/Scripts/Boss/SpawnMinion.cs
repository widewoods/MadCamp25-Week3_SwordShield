using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
  [SerializeField] private GameObject minionPrefab;

  public void Spawn(Vector3 spawnPosition)
  {
    GameObject go = Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
    go.name = "Minion_Tower";
  }
}
