using UnityEngine;

public class CutsceneEventForwarder : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    private Animator anim;

    public void EnterFloor()
    {
        uiController.CallFloorScene();
    }

    void OnEnable(){
        anim = GetComponent<Animator>();
        anim.SetTrigger("Start");
    }

}
