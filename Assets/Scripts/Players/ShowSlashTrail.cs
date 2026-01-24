using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSlashTrail : MonoBehaviour
{
  [SerializeField] private TrailRenderer trailRenderer;
  public void StartTrail()
  {
    trailRenderer.Clear();
    trailRenderer.emitting = true;
  }

  public void EndTrail()
  {
    trailRenderer.emitting = false;
    trailRenderer.Clear();
  }
}
