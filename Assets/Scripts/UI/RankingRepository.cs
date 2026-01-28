using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RankingRepository
{
    private string path;

    public RankingRepository()
    {
        path = Path.Combine(Application.persistentDataPath, "ranking.json");
    }

    // =========================
    // Add (랭킹 등록)
    // =========================
    public void Add(string name, string floor, int score)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("[RankingRepository] Name is empty. Skip Add.");
            return;
        }

        RankingDB db = Load();
        db.entries.Add(new RankingEntry(name, floor, score));
        Save(db);
    }

    // =========================
    // Load (Raw)
    // =========================
    public RankingDB Load()
    {
        if (!File.Exists(path))
            return new RankingDB();

        string json = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(json))
            return new RankingDB();

        return JsonUtility.FromJson<RankingDB>(json) ?? new RankingDB();
    }

    // =========================
    // Load + Sort (UI에서 사용)
    // floor: Floor3 > Floor2 > Floor1
    // =========================
    public List<RankingEntry> LoadSorted(int topN = 50)
    {
        RankingDB db = Load();

        var sorted = db.entries
            .OrderByDescending(e => e.floor)        // Floor3 > Floor2 > Floor1
            .ThenByDescending(e => e.score)         // 점수 내림차순
            .ThenByDescending(e => e.createdAt)     // 최신 기록 우선
            .ToList();

        if (topN > 0 && sorted.Count > topN)
            sorted = sorted.Take(topN).ToList();

        return sorted;
    }

    // =========================
    // Save
    // =========================
    private void Save(RankingDB db)
    {
        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(path, json);
    }

    // =========================
    // Utils
    // =========================
    public void Clear()
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public string GetFilePath()
    {
        return path;
    }
}
