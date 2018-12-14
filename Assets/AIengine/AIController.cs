using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    patrol,
    attack
}
public class AIController : MonoBehaviour {
    [SerializeField] private bool facingRight;
    [SerializeField] private GameObject PatternPointPrefab;
    [SerializeField] private List<GameObject> PatternPointList;
    [SerializeField] private AIState currState;
    [SerializeField] private float runSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float PatternPointXEpsilon;
    [SerializeField] private float PatternPointYEpsilon;
    
    private Rigidbody2D rigid;
    private PatternPoint currPatternPoint;
    private int currPatternPointIndex = 0;
    private Animator anim;
    public AIState CurrState
    {
        get
        {
            return currState;
        }

        set
        {
            currState = value;
        }
    }
    private Vector3 direction;

    // Use this for initialization
    void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        currPatternPoint = PatternPointList[currPatternPointIndex].GetComponent<PatternPoint>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (currState)
        {
            case AIState.patrol:
                patrolState();
                break;
            case AIState.attack:
                attackState();
                break;
        }
            


	}
    private void FixedUpdate()
    {
        rigid.AddForce(direction * rigid.mass * runSpeed * acceleration);
    }
    void patrolState()
    {
        if (PatternPointList == null || PatternPointList.Count == 0) return;
        if (Mathf.Abs(gameObject.transform.position.x - PatternPointList[currPatternPointIndex].transform.position.x) < PatternPointXEpsilon &&
            Mathf.Abs(gameObject.transform.position.y - PatternPointList[currPatternPointIndex].transform.position.y) < PatternPointYEpsilon)
        {
            Debug.Log("+");
            anim.SetBool("runBool", false);
            direction = Vector3.zero;
            
            if (currPatternPoint.HoldDownEnd)
            {
                currPatternPoint.HoldDownEnd = false;
                nextPatternPoint();
            }
            else
            {
                currPatternPoint.StartTick();

            }
        }
        else
        {
            anim.SetBool("runBool", true);
            if (PatternPointList[currPatternPointIndex].transform.position.x < gameObject.transform.position.x)
            {
                direction = Vector3.left;
                if (facingRight) Flip();
            } 
            else
            {
                if (!facingRight) Flip();
                direction = Vector3.right;
            }
                
        }
    }
    void Flip() // отражение по горизонтали
    {
        
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void attackState()
    {

    }
    void nextPatternPoint()
    {
        if (currPatternPointIndex >= PatternPointList.Count - 1)
            currPatternPointIndex = 0;
        else
            currPatternPointIndex++;

        currPatternPoint = PatternPointList[currPatternPointIndex].GetComponent<PatternPoint>();
    }
    public void AddPatternPoint()
    {
        Instantiate(PatternPointPrefab);
    }
}
