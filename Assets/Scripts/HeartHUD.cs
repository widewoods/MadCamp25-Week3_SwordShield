using UnityEngine;
using UnityEngine.UI;

public class HeartHUD : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int maxHearts = 3; // 하트 개수
    [SerializeField] private int hp;            // 0 ~ maxHearts*2 (반칸 단위)

    [Header("UI")]
    [SerializeField] private Image[] heartImages; // 크기 = maxHearts (왼쪽부터)
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    private int MaxHP => maxHearts * 2;

    private void Awake()
    {
        // 시작 풀피로
        hp = MaxHP;
        UpdateHearts();
    }

    public void SetHP(int newHp)
    {
        hp = Mathf.Clamp(newHp, 0, MaxHP);
        UpdateHearts();
    }

    public void TakeHit(int halfUnits = 1)
    {
        hp = Mathf.Clamp(hp - halfUnits, 0, MaxHP);
        UpdateHearts();
    }

    public void Heal(int halfUnits = 1)
    {
        hp = Mathf.Clamp(hp + halfUnits, 0, MaxHP);
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        // 각 하트에 남아있는 half-unit 계산
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = Mathf.Clamp(hp - (i * 2), 0, 2); // 0,1,2

            if (heartValue == 2) heartImages[i].sprite = fullHeart;
            else if (heartValue == 1) heartImages[i].sprite = halfHeart;
            else heartImages[i].sprite = emptyHeart;
        }
    }
}
