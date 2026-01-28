using TMPro;
using UnityEngine;

public class RankingRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text floorText;
    [SerializeField] private TMP_Text scoreText;

    public void Bind(int rank, string name, string floor, int score)
    {
        if (rankText) rankText.text = rank.ToString();
        if (nameText) nameText.text = name;
        if (floorText) floorText.text = floor;
        if (scoreText) scoreText.text = score.ToString("N0");
    }
}
