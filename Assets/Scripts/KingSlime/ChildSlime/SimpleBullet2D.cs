using UnityEngine;

public class SimpleBullet2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Life")]
    [SerializeField] private float lifeTime = 4f;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
