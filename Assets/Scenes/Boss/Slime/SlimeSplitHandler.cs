using System;
using UnityEngine;

public class SlimeSplitHandler : MonoBehaviour
{
    [Header("Split Settings")]
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private float scaleMultiplier = 0.7f;
    [SerializeField] private float spawnOffsetRadius = 0.6f;

    private SlimeBossController controller;

    private void Awake()
    {
        controller = GetComponent<SlimeBossController>();
    }

    /// <summary>
    /// 슬라임 분열 실행
    /// </summary>
    public void Execute(Action onFinished)
    {
        if (slimePrefab == null || controller == null)
        {
            onFinished?.Invoke();
            Destroy(gameObject);
            return;
        }

        int nextSplitCount = controller.splitCount + 1;

        for (int i = 0; i < 2; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * spawnOffsetRadius;

            GameObject child = Instantiate(
                slimePrefab,
                (Vector2)transform.position + offset,
                Quaternion.identity
            );

            // 스케일 감소
            child.transform.localScale =
                transform.localScale * scaleMultiplier;

            // 스탯/상태 전달
            var childController = child.GetComponent<SlimeBossController>();
            if (childController != null)
            {
                childController.maxHP = controller.maxHP * 0.5f;
                childController.currentHP = controller.currentHP * 0.5f;
                childController.splitCount = nextSplitCount;
            }
        }

        // 원본 슬라임 제거
        Destroy(gameObject);

        // Split은 보통 자기 자신이 사라지므로
        // onFinished는 "논리적으로" 바로 호출
        onFinished?.Invoke();
    }
}
