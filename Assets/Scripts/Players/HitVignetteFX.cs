using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HitVignetteFX : MonoBehaviour
{
  [SerializeField] private Volume volume;

  [Header("Flash Settings")]
  public Color hitColor = new Color(1f, 0f, 0f, 1f);
  public float peakIntensity = 0.1f;
  public float inTime = 0.05f;
  public float outTime = 0.20f;

  private Vignette vignette;
  private Coroutine co;

  void Awake()
  {
    if (!volume) volume = FindObjectOfType<Volume>();

    if (volume && volume.profile.TryGet(out vignette))
    {
      vignette.active = true;
      vignette.color.Override(hitColor);
      vignette.intensity.Override(0f);
    }
    else
    {
      Debug.LogWarning("Vignette not found in Volume profile.");
    }
  }

  public void Flash()
  {
    if (vignette == null) return;
    if (co != null) StopCoroutine(co);
    co = StartCoroutine(FlashRoutine());
  }

  private IEnumerator FlashRoutine()
  {
    // ramp up
    float t = 0f;
    while (t < inTime)
    {
      t += Time.unscaledDeltaTime;
      vignette.intensity.value = Mathf.Lerp(0f, peakIntensity, t / inTime);
      yield return null;
    }
    vignette.intensity.value = peakIntensity;

    // ramp down
    t = 0f;
    while (t < outTime)
    {
      t += Time.unscaledDeltaTime;
      vignette.intensity.value = Mathf.Lerp(peakIntensity, 0f, t / outTime);
      yield return null;
    }
    vignette.intensity.value = 0f;
    co = null;
  }
}
