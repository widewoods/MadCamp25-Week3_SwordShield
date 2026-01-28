using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TwoTargetClampCamera2D : MonoBehaviour
{
  [Header("Targets")]
  public Transform playerA;
  public Transform playerB;

  [Header("Map Bounds (World Space)")]
  // Define your playable rectangle in world coordinates
  public Rect mapRect = new Rect(-10, -10, 20, 20);

  [Header("Follow")]
  public float positionSmoothTime = 0.15f;  // smaller = snappier
  public Vector2 screenPaddingWorld = new Vector2(2.5f, 2.0f); // extra space around players (world units)
  public Vector2 cameraOffset = Vector2.zero;

  [Header("Zoom")]
  public float zoomSmoothTime = 0.20f;
  public float minOrthoSize = 5f;
  public float maxOrthoSize = 14f;

  [Header("Optional look-ahead (reactive feel)")]
  public bool useLookAhead = true;
  public float lookAheadStrength = 0.35f;   // 0..1-ish
  public float lookAheadMax = 3.0f;

  Camera cam;
  Vector3 posVel;     // SmoothDamp velocity
  float zoomVel;      // SmoothDamp velocity

  Vector3 prevA, prevB;

  void Awake()
  {
    cam = GetComponent<Camera>();
    cam.orthographic = true;

    if (playerA) prevA = playerA.position;
    if (playerB) prevB = playerB.position;
  }

  void LateUpdate()
  {
    if (!playerA || !playerB) return;

    Vector3 a = playerA.position;
    Vector3 b = playerB.position;

    // Center point between players
    Vector3 center = (a + b) * 0.5f;

    // Look-ahead based on average velocity (feels more reactive)
    if (useLookAhead)
    {
      Vector3 velA = (a - prevA) / Mathf.Max(Time.deltaTime, 1e-6f);
      Vector3 velB = (b - prevB) / Mathf.Max(Time.deltaTime, 1e-6f);
      Vector3 avgVel = (velA + velB) * 0.5f;

      Vector3 la = avgVel * lookAheadStrength * 0.1f; // scale to taste
      la = Vector3.ClampMagnitude(la, lookAheadMax);
      center += la;
    }

    prevA = a;
    prevB = b;

    // Desired zoom to fit both players (+ padding)
    float targetSize = ComputeTargetOrthoSize(a, b);

    // Smooth zoom
    cam.orthographicSize = Mathf.SmoothDamp(
        cam.orthographicSize, targetSize, ref zoomVel, zoomSmoothTime);

    // Desired camera position
    Vector3 desired = new Vector3(center.x + cameraOffset.x, center.y + cameraOffset.y, transform.position.z);

    // Clamp camera to map so it doesn't show outside
    desired = ClampToMap(desired, cam.orthographicSize);

    // Smooth position
    transform.position = Vector3.SmoothDamp(transform.position, desired, ref posVel, positionSmoothTime);
  }

  float ComputeTargetOrthoSize(Vector3 a, Vector3 b)
  {
    // Half distances between players
    float halfHeight = Mathf.Abs(a.y - b.y) * 0.5f + screenPaddingWorld.y;
    float halfWidth = Mathf.Abs(a.x - b.x) * 0.5f + screenPaddingWorld.x;

    // For orthographic camera:
    // orthographicSize is half of vertical size in world units
    float sizeByHeight = halfHeight;

    // Horizontal must fit: halfWidth <= orthoSize * aspect
    float sizeByWidth = halfWidth / Mathf.Max(cam.aspect, 1e-6f);

    float desired = Mathf.Max(sizeByHeight, sizeByWidth);
    desired = Mathf.Clamp(desired, minOrthoSize, maxOrthoSize);
    return desired;
  }

  Vector3 ClampToMap(Vector3 desired, float orthoSize)
  {
    float camHalfH = orthoSize;
    float camHalfW = orthoSize * cam.aspect;

    // Map edges
    float left = mapRect.xMin;
    float right = mapRect.xMax;
    float bottom = mapRect.yMin;
    float top = mapRect.yMax;

    // Clamp range for camera center so camera view stays inside map
    float minX = left + camHalfW;
    float maxX = right - camHalfW;
    float minY = bottom + camHalfH;
    float maxY = top - camHalfH;

    // If map is smaller than camera view, lock to center of map
    if (minX > maxX) desired.x = (left + right) * 0.5f;
    else desired.x = Mathf.Clamp(desired.x, minX, maxX);

    if (minY > maxY) desired.y = (bottom + top) * 0.5f;
    else desired.y = Mathf.Clamp(desired.y, minY, maxY);

    return desired;
  }

#if UNITY_EDITOR
  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Vector3 c = new Vector3(mapRect.center.x, mapRect.center.y, 0);
    Vector3 s = new Vector3(mapRect.size.x, mapRect.size.y, 0);
    Gizmos.DrawWireCube(c, s);
  }
#endif
}
