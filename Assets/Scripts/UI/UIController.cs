using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public enum UIState{
        Main,
        HUD,
        Exit,
        Over,
        Cut,
        Rank,
        Register,
        Loading
    }

    public enum SceneState{
        UI,
        Floor1,
        Floor2,
        Floor3
    }

    public UIState uistate;
    public SceneState scstate;
    public CutSceneController CSController;
    public int finalScore;

    [Header("UI Pages")]
    [SerializeField] public GameObject Main;
    [SerializeField] public GameObject HUD;
    [SerializeField] public GameObject Exit;
    [SerializeField] public GameObject Over;
    [SerializeField] public GameObject Cut;
    [SerializeField] public GameObject Rank;
    [SerializeField] public GameObject Register;
 
    [Header("Refs")]
    [SerializeField] public ScoreController ScoreCon;
    [SerializeField] public TMP_Text Over_Title;
    [SerializeField] public TMP_Text NameField;

    
    //Callback Functions
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnemyHealth.OnBossDeath += CallLoadingScene;
        InBetweenStageManager.OnReady += CallNextFloor;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EnemyHealth.OnBossDeath -= CallLoadingScene;
        InBetweenStageManager.OnReady -= CallNextFloor;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "FirstFloor-Slime")
        {
            // FirstFloor에 맞게 UI 상태 전환
            ChangeUI(UIState.HUD);
            CSController.UpdateFloor(1);
            // CallNextFloor();
            // 필요하면 여기서 Player/Managers 재참조
            // player = FindObjectOfType<PlayerController>();
        }
        else if (scene.name == "SecondFloor"){
            ChangeUI(UIState.HUD);
            CSController.UpdateFloor(2);
            // CallNextFloor();
        }
        else if (scene.name == "ThirdFloor"){
            ChangeUI(UIState.HUD);
            CSController.UpdateFloor(3);
        }
        else if (scene.name == "UIScene")
        {
            ChangeUI(UIState.Main);
        }
        else if(scene.name == "Load1" || scene.name == "Load2")
        {
            ChangeUI(UIState.Loading);
        }
    }

    void Awake()
    {
        Time.timeScale = 1f;

        uistate = UIState.Main;
        scstate = SceneState.UI;
        CSController = GetComponent<CutSceneController>();


        GameObject gmObj = GameObject.Find("GameManager");
        ScoreCon = gmObj.GetComponent<ScoreController>();
        
        SetActiveUI(UIState.Main, true);
        SetActiveUI(UIState.HUD, false);
        SetActiveUI(UIState.Exit, false);
        SetActiveUI(UIState.Over, false);
        SetActiveUI(UIState.Cut, false);
    }




    // Manage UIState
    public void ChangeUIbyInt(int state){
        ChangeUI((UIState)state);
    }

    public void ChangeUI(UIState state){
        if(uistate == state) return;
        SetActiveUI(uistate, false);
        SetActiveUI(state, true);
        uistate = state;
    }

    public void SetActiveUI(UIState state, bool active){
        if (state == UIState.Main) Main.SetActive(active);
        else if (state == UIState.HUD) HUD.SetActive(active);
        else if (state == UIState.Exit) Exit.SetActive(active);
        else if (state == UIState.Over) Over.SetActive(active);
        else if (state == UIState.Cut) Cut.SetActive(active);
        else if (state == UIState.Rank) Rank.SetActive(active);
        else if (state == UIState.Register) Register.SetActive(active);
        else if (state == UIState.Loading) {}
        else Debug.LogError($"Invalid UI State: {state}");
    }

    // Call Scene Functions
    public void CallFloorScene(){
        int floor = CSController.currentFloor;
        if (floor == 1) SceneManager.LoadScene("FirstFloor-Slime");
        else if (floor == 2) SceneManager.LoadScene("SecondFloor");
        else if (floor == 3) SceneManager.LoadScene("ThirdFloor");
        else Debug.LogError($"Invalid floor number: {floor}");
    }

    public void GameOver(){
        finalScore = ScoreCon.FinalizeOnGameOver();
        Over_Title.text += "\nScore: " + finalScore.ToString(); 
        Time.timeScale = 0f;
        ChangeUI(UIState.Over);
    }

    public void CallRegister(){
        ChangeUI(UIState.Register);
    }

    public void RegistRank(){
        // Data
        string Name = NameField.text;
        string Floor = "Floor" + CSController.currentFloor.ToString();
        int Score = finalScore;


        // Register
        RankingRepository repo = new RankingRepository();
        repo.Add(Name, Floor, Score);
        
        Debug.Log(Application.persistentDataPath);
        Debug.Log($"Rank registered: {Name}, {Floor}F, {Score}");
    }

    public void CallUIScene(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("UIScene");
    }


    public void CallNextFloor(){
        int floor = CSController.currentFloor;
        if (floor == 1){
            CSController.UpdateFloor(2);
            ChangeUI(UIState.Cut);
        }
        else if (floor == 2){
            CSController.UpdateFloor(3);
            ChangeUI(UIState.Cut);
        }
        else if (floor == 3){
            CallUIScene();
        }
    }

    public void CallLoadingScene(int floor)
    {
      if(floor == 1)
    {
      SceneManager.LoadScene("Load1");
    }
    else if(floor == 2)
    {
      SceneManager.LoadScene("Load2");
    }
    else
    {
      
    }
      
    }
    
}
