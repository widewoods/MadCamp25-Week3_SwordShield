using System;
using UnityEngine;

public class SlimeSplitHandler : MonoBehaviour
{
    [Header("Split Settings")]
    [SerializeField] private GameObject slimePrefab1;
    [SerializeField] private GameObject slimePrefab2;
    [SerializeField] private float spawnOffsetRadius = 1.5f;

    private SlimeBossController controller;
    private GameObject[] slimePrefabs;

    private void Awake()
    {
        controller = GetComponent<SlimeBossController>();
        slimePrefabs = new GameObject[] {slimePrefab1, slimePrefab2};
    }

    /// <summary>
    /// 슬라임 분열 실행
    /// </summary>
    public void SplitExecute()
    {
        if (slimePrefabs == null || controller == null)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            Vector2 offset = new Vector2 ((float)Math.Pow(-1, i) * spawnOffsetRadius, 0);

            GameObject child = Instantiate(
                slimePrefabs[i],
                offset,
                Quaternion.identity
            );
        }

        // 원본 슬라임 제거
        Destroy(gameObject);
    }
}
