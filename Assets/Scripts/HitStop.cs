using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
  public static HitStop Instance;

  void Awake()
  {
    if (Instance != null) { Destroy(gameObject); return; }
    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  Coroutine co;

  public void Do(float timeScale, float durationRealtime)
  {
    if (co != null) StopCoroutine(co);
    co = StartCoroutine(Routine(timeScale, durationRealtime));
  }

  IEnumerator Routine(float timeScale, float durationRealtime)
  {
    float oldScale = Time.timeScale;
    float oldFixed = Time.fixedDeltaTime;

    Time.timeScale = timeScale;
    Time.fixedDeltaTime = oldFixed * timeScale; // keeps physics stable

    yield return new WaitForSecondsRealtime(durationRealtime);

    Time.timeScale = oldScale;
    Time.fixedDeltaTime = oldFixed;

    co = null;
  }
}
