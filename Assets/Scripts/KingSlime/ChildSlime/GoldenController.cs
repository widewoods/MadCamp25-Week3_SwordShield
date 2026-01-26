using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenController : MonoBehaviour
{   
    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;
    public SlimeAttack_SuperDash SupaDash;

    [Header("Stats")]
    public float MoveSpeed = 2.0f;

    private bool isBusy = false;
    public enum ChildState{
        Move,
        Attack,
        Dead
    }
    public ChildState currentState = ChildState.Move;
    private Coroutine StateCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //StateCoroutine = StartCoroutine(StateRoutine());
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        SupaDash = GetComponent<SlimeAttack_SuperDash>();
        currentState = ChildState.Attack;
    }
    // Update is called once per frame
    void Update()
    {
        // if (currentState == ChildState.Move){
        //     StartMove();
        //     return;
        // }

        if (currentState == ChildState.Attack){
            if (isBusy) return;

            isBusy = true;
            //rb.velocity = Vector2.zero;
            Debug.Log("Attack");
            animator.SetTrigger("Attack");
            SupaDash.StartRicochetRandom(OnActionFinished);
        }
    }

    // On-Action Functions
    public void StartMove(){
        Vector2 Dir = SelectTargetDir();
        rb.velocity = Dir * MoveSpeed;
    }

    public Vector2 SelectTargetDir()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        if (players == null || players.Length == 0)
            return Vector2.zero;

        Vector3 myPos = transform.position;

        Transform closest = null;
        float minSqrDist = float.MaxValue;

        foreach (var p in players)
        {
            if (p == null || !p.activeInHierarchy)
                continue;

            Vector3 diff = p.transform.position - myPos;
            float sqrDist = diff.sqrMagnitude;

            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closest = p.transform;
            }
        }

        if (closest == null)
            return Vector2.zero;

        Vector2 dir = (closest.position - myPos);
        return dir.normalized;
    }

    public void OnActionFinished(){
        // currentState = ChildState.Move;
        // animator.SetTrigger("Move");
        // isBusy = false;
    }

    private IEnumerator StateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (currentState == ChildState.Move && !isBusy)
            {
                currentState = ChildState.Attack;
            }
        }
    }

}
