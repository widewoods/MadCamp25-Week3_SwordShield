using UnityEngine;

public class SimpleBullet2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Life")]
    [SerializeField] private float lifeTime = 4f;

    [Header("Collision")]
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private LayerMask hitMask; // 비워두면 "뭐든" 충돌 시 삭제

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// dir 방향으로 speed 속도로 발사
    /// </summary>
    public void Launch(Vector2 dir, float speed)
    {
        if (rb != null)
        {
            rb.velocity = dir.normalized * speed;
        }

        // 수명 끝나면 자동 삭제
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyOnHit) return;

        // hitMask가 비어있으면 그냥 삭제
        if (hitMask.value == 0)
        {
            Destroy(gameObject);
            return;
        }

        // hitMask에 포함된 레이어면 삭제
        int otherLayerMask = 1 << other.gameObject.layer;
        if ((hitMask.value & otherLayerMask) != 0)
        {
            Destroy(gameObject);
        }
    }
}
