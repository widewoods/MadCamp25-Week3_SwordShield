using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] public int floor = 1;

    [Header("Sprites")]
    [SerializeField] public Sprite SlimeKing;
    [SerializeField] public Sprite Golem;
    [SerializeField] public Sprite Magician;

    [Header("Cut_Titles")]
    [SerializeField] public string textFloor1 = "Floor1\n- Slime King -";
    [SerializeField] public string textFloor2 = "Floor2\n- Giant Golem -";
    [SerializeField] public string textFloor3 = "Floor3\n- Lord of Sword & Shield -";

    [Header("Offsets")]
    [SerializeField] public Vector2 offset1 = new Vector2(50, 90);
    [SerializeField] public Vector2 offset2 = new Vector2(0, 0);
    [SerializeField] public Vector2 offset3 = new Vector2(0, 0);

    [Header("GameObjects")]
    [SerializeField] public Image Boss;
    [SerializeField] public TMP_Text Exit_Title;
    [SerializeField] public TMP_Text Over_Title;
    [SerializeField] public TMP_Text Cut_Title;

    public int currentFloor = 0;

    void Awake(){
        UpdateFloor(floor);
    }

    public void UpdateFloor(int input){
        floor = input;
        if(currentFloor == floor) return;
        Debug.Log($"Change Floor: {input}");
        currentFloor = floor;
        ChangeState(currentFloor);
    }

    public void ChangeState(int floor){
        if (floor == 1){
            Boss.sprite = SlimeKing;
            Boss.rectTransform.anchoredPosition = offset1;
            ChangeTitle(textFloor1);
        }
        else if (floor == 2){
            Boss.sprite = Golem;
            Boss.rectTransform.anchoredPosition = offset2;
            ChangeTitle(textFloor2);
        }
        else if (floor == 3){
            Boss.sprite = Magician;
            Boss.rectTransform.anchoredPosition = offset3;
            ChangeTitle(textFloor3);
        }
        else
            Debug.LogError($"Invalid floor number:{floor}");
    }

    public void ChangeTitle(string text){
        Exit_Title.text = text;
        Over_Title.text = text;
        Cut_Title.text = text;
    }
}
