using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance { get; private set; }

    // =========================
    // Tuning (인스펙터에서 조절)
    // =========================
    [Header("Scoring Tuning")]
    [SerializeField] private int floorClearBaseScore = 10000;      // 층 클리어 기본점수
    [SerializeField] private int hpBonusMax = 2000;                // HP 100%일 때 보너스
    [SerializeField] private float timePenaltyPerSecond = 10f;     // 층당 초당 페널티
    [SerializeField] private float survivalScorePerSecond = 100f;  // 0층(1층도 못깸) 생존 점수

    [Header("Scene Names")]
    [SerializeField] private string uiSceneName = "UIScene";
    [SerializeField] private string firstFloorSceneName = "FirstFloor-Slime";
    [SerializeField] private string secondFloorSceneName = "SecondFloor";
    [SerializeField] private string thirdFloorSceneName = "ThirdFloor";

    // =========================
    // Runtime State
    // =========================
    [Header("Runtime State (ReadOnly)")]
    [SerializeField] private int currentFloor = 0;     // 0=UI/기타, 1~3=층
    [SerializeField] private int clearedFloor = 0;     // 0~3
    [SerializeField] private int totalScore = 0;       // 1층 이상에서 누적 점수
    [SerializeField] private bool isRunning = false;

    // 타이머: 전체 런 / 층
    [SerializeField] private float runTime = 0f;
    [SerializeField] private float floorTime = 0f;

    // =========================
    // Last Result (for UI)
    // =========================
    [Header("Last Result (ReadOnly)")]
    [SerializeField] private int lastScore = 0;
    [SerializeField] private float lastSurvivalSeconds = 0f;

    // =========================
    // Unity Lifecycle
    // =========================
    private void Awake()
    {
        // Singleton + DontDestroyOnLoad
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!isRunning) return;

        // timeScale=0(GameOver) 상태에서도 타이머가 필요하면 unscaledDeltaTime 사용이 안전
        float dt = Time.unscaledDeltaTime;
        runTime += dt;
        floorTime += dt;
    }

    // =========================
    // Scene Hook (층 판별)
    // =========================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == firstFloorSceneName)
        {
            // 첫 층 진입 = 새 런 시작
            StartRun();
            EnterFloor(1);
        }
        else if (scene.name == secondFloorSceneName)
        {
            EnterFloor(2);
        }
        else if (scene.name == thirdFloorSceneName)
        {
            EnterFloor(3);
        }
        else if (scene.name == uiSceneName)
        {
            // UI 씬에서는 currentFloor = 0 처리
            currentFloor = 0;

            // 주의: 여기서 점수 확정하지 않음.
            // 점수 확정은 "죽는 순간/클리어 순간"에 FinalizeOnGameOver/FinalizeOnClear를 호출해서 처리.
        }
        else
        {
            // 기타 씬이면 floor로 보지 않음
            currentFloor = 0;
        }
    }

    private void EnterFloor(int floor)
    {
        currentFloor = floor;
        StartFloor();
    }

    // =========================
    // Run / Floor Control
    // =========================
    public void StartRun()
    {
        clearedFloor = 0;
        totalScore = 0;

        runTime = 0f;
        floorTime = 0f;

        isRunning = true;

        lastScore = 0;
        lastSurvivalSeconds = 0f;
    }

    public void StartFloor()
    {
        floorTime = 0f;
    }

    // =========================
    // Boss / Clear / GameOver API
    // =========================
    /// <summary>
    /// 보스가 죽는 순간 호출.
    /// hpRatio: currentHP / maxHP (0~1)
    /// return: 이번 층에서 얻은 점수(gained)
    /// </summary>
    public int OnBossDefeated(float hpRatio)
    {
        if (!isRunning)
        {
            Debug.LogWarning("[ScoreController] OnBossDefeated called but isRunning is false.");
            return 0;
        }

        if (currentFloor <= 0)
        {
            Debug.LogWarning("[ScoreController] OnBossDefeated called but currentFloor is 0 (not in floor scene).");
            return 0;
        }

        hpRatio = Mathf.Clamp01(hpRatio);

        int hpBonus = Mathf.RoundToInt(hpRatio * hpBonusMax);
        int timePenalty = Mathf.RoundToInt(floorTime * timePenaltyPerSecond);

        int gained = floorClearBaseScore + hpBonus - timePenalty;
        if (gained < 0) gained = 0; // 음수 방지 (원하면 제거)

        totalScore += gained;
        clearedFloor = Mathf.Max(clearedFloor, currentFloor);

        // 다음 층 진입 대비 (또는 다음 씬에서 StartFloor가 호출되어도 상관없음)
        floorTime = 0f;

        return gained;
    }

    /// <summary>
    /// 플레이어가 죽는 순간 호출 (게임오버 확정).
    /// - 1층도 못 깼으면: 생존시간 점수로 lastScore 결정
    /// - 1층 이상이면: 누적 totalScore가 lastScore
    /// </summary>
    public int FinalizeOnGameOver()
    {
        if (!isRunning) return lastScore;

        isRunning = false;
        lastSurvivalSeconds = runTime;

        if (clearedFloor <= 0)
        {
            lastScore = Mathf.Max(0, Mathf.RoundToInt(lastSurvivalSeconds * survivalScorePerSecond));
        }
        else
        {
            lastScore = totalScore;
        }

        Debug.Log($"FinalScore: {lastScore} / SurvivalTime: {lastSurvivalSeconds}");
        return lastScore;
    }

    /// <summary>
    /// 3층까지 완료(완클) 확정 시 호출.
    /// </summary>
    public int FinalizeOnClear()
    {
        if (!isRunning) return lastScore;

        isRunning = false;
        lastSurvivalSeconds = runTime;
        lastScore = totalScore;

        return lastScore;
    }

    // =========================
    // Getters (UI에서 사용)
    // =========================
    public int GetLastScore() => lastScore;
    public int GetTotalScore() => totalScore;
    public int GetCurrentFloor() => currentFloor;
    public int GetClearedFloor() => clearedFloor;
    public float GetRunTime() => runTime;
    public float GetFloorTime() => floorTime;
    public float GetLastSurvivalSeconds() => lastSurvivalSeconds;
}
