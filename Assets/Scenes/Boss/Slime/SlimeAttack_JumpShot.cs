using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlimeAttack_JumpShot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 12;
    [SerializeField] private float bulletSpeed = 7f;
    [SerializeField] private float spreadLen = 2f;

    private bool isRunning = false;
    
    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();
    }

    private void ShootRadial()
    {
        if (bulletPrefab == null) return;

        Vector2 origin = boxCollider != null
            ? (Vector2)boxCollider.bounds.center
            : rb.position;
        float step = 360f / Mathf.Max(1, bulletCount);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * step;
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ).normalized;

            Vector2 init_pos = origin + dir * spreadLen;
            GameObject b = Instantiate(bulletPrefab, init_pos, Quaternion.identity);

            if (b.TryGetComponent<SimpleBullet2D>(out var bullet))
            {
                bullet.Launch(dir, bulletSpeed);
            }
            else
            {
                var brb = b.GetComponent<Rigidbody2D>();
                if (brb != null) brb.velocity = dir * bulletSpeed;
            }
        }
    }

}
