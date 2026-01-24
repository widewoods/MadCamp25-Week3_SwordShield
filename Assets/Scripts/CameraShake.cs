using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  Vector3 originalLocalPos;
  Coroutine co;

  void Awake() => originalLocalPos = transform.localPosition;

  public void Shake(float duration = 0.12f, float magnitude = 0.15f)
  {
    if (co != null) StopCoroutine(co);
    co = StartCoroutine(ShakeRoutine(duration, magnitude));
  }

  IEnumerator ShakeRoutine(float duration, float magnitude)
  {
    float t = 0f;
    while (t < duration)
    {
      t += Time.deltaTime;
      float damper = 1f - (t / duration);
      Vector2 r = Random.insideUnitCircle * magnitude * damper;
      transform.localPosition = originalLocalPos + new Vector3(r.x, r.y, 0f);
      yield return null;
    }
    transform.localPosition = originalLocalPos;
    co = null;
  }
}
