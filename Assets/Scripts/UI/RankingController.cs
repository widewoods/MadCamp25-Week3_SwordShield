using UnityEngine;

public class RankingController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform content;
    [SerializeField] private RankingRowView rowPrefab;
    [SerializeField] private int topN = 20;

    private RankingRepository repo;

    // 현재 정렬 상태
    private RankingSortKey sortKey = RankingSortKey.Floor;
    private bool descending = true;

    private void Awake()
    {
        repo = new RankingRepository();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        var list = repo.LoadSorted();

        // 기존 row 제거
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);

        // 새로 생성 + rank 부여
        for (int i = 0; i < list.Count; i++)
        {
            var e = list[i];
            var row = Instantiate(rowPrefab, content);
            row.Bind(i + 1, e.name, e.floor, e.score);
        }
    }

    // ===== 버튼에 연결할 함수들 =====

    public void SortByFloor()
    {
        ToggleOrChangeKey(RankingSortKey.Floor);
    }

    public void SortByScore()
    {
        ToggleOrChangeKey(RankingSortKey.Score);
    }

    public void SortByRecent()
    {
        ToggleOrChangeKey(RankingSortKey.Recent);
    }

    private void ToggleOrChangeKey(RankingSortKey nextKey)
    {
        if (sortKey == nextKey)
        {
            // 같은 키를 다시 누르면 오름/내림 토글
            descending = !descending;
        }
        else
        {
            sortKey = nextKey;
            descending = true; // 새 키는 기본 내림차순(원하면 false로)
        }

        Refresh();
    }
}
