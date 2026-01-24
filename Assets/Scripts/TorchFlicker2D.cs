using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker2D : MonoBehaviour
{
  [SerializeField] Light2D light2D;

  [Header("Intensity")]
  [SerializeField] float baseIntensity = 1.0f;
  [SerializeField] float intensityAmp = 0.25f;

  [Header("Frequency")]
  [SerializeField] float speed = 8f;          // flicker speed
  [SerializeField] float noiseSpeed = 1.7f;   // slow drift

  [Header("Radius (optional)")]
  [SerializeField] bool flickerRadius = true;
  [SerializeField] float baseOuterRadius = 3.0f;
  [SerializeField] float radiusAmp = 0.25f;

  float seed;

  void Awake()
  {
    if (!light2D) light2D = GetComponent<Light2D>();
    seed = Random.value * 1000f;
    if (light2D != null)
    {
      baseIntensity = light2D.intensity;
      baseOuterRadius = light2D.pointLightOuterRadius;
    }
  }

  void Update()
  {
    if (!light2D) return;

    float t = Time.time;

    // two-layer noise = less "sine-y"
    float n1 = Mathf.PerlinNoise(seed, t * noiseSpeed);
    float n2 = Mathf.PerlinNoise(seed + 31.7f, t * speed);

    float n = (n1 * 0.6f + n2 * 0.4f); // 0..1
    float centered = (n - 0.5f) * 2f;  // -1..1

    light2D.intensity = baseIntensity + centered * intensityAmp;

    if (flickerRadius)
      light2D.pointLightOuterRadius = baseOuterRadius + centered * radiusAmp;
  }
}
