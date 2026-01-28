using System;
using System.Collections.Generic;

[Serializable]
public class RankingEntry
{
    public string name;
    public string floor;
    public int score;
    public long createdAt;

    public RankingEntry(string name, string floor, int score)
    {
        this.name = name;
        this.floor = floor;
        this.score = score;
        this.createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}

[Serializable]
public class RankingDB
{
    public List<RankingEntry> entries = new List<RankingEntry>();
}

public enum RankingSortKey
{
    Floor,
    Score,
    Recent
}