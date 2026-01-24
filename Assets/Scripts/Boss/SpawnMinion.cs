using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
  [SerializeField] private GameObject minionPrefab;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.M))
    {
      for (int x = -2; x <= 2; x += 4)
      {
        for (int y = -2; y <= 2; y += 4)
        {
          Vector3 spawnPos = transform.position + new Vector3(x * 4f, y * 2f, 0f);
          Spawn(spawnPos);
        }
      }
    }
  }

  public void Spawn(Vector3 spawnPosition)
  {
    Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
  }
}
