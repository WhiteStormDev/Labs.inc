
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Character2DControl : MonoBehaviour {

    [SerializeField] private Animator playerAnimator;
	[SerializeField] private float speed = 1.5f; // скорость движения
	[SerializeField] private float acceleration = 100; // ускорение
	[SerializeField] private float jumpForce = 5; // сила прыжка
	[SerializeField] private float jumpDistance = 0.75f; // расстояние от центра объекта, до поверхности (определяется вручную в зависимости от размеров спрайта)
	[SerializeField] private bool facingRight = true; // в какую сторону смотрит персонаж на старте?
	[SerializeField] private KeyCode jumpButton = KeyCode.Space; // клавиша для прыжка
    [SerializeField] private bool canMove;
    [SerializeField] private float heroVelocityEpsilonForJump = 0.1f;
    //public static LadderManager ladderManager;
    private Vector3 direction = Vector3.zero;
	private int layerMask;
	private Rigidbody2D body;

    public bool CanMove
    {
        get
        {
            return canMove;
        }

        set
        {
            canMove = value;
        }
    }

    void Start () 
	{
        //if (ladderManager == null)
        //    ladderManager = GameObject.FindObjectOfType<LadderManager>();
        canMove = true;
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;                             
		layerMask = 1 << gameObject.layer | 1 << 2;
		layerMask = ~layerMask;
	}

	bool GetJump() // проверяем, есть ли коллайдер под ногами
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, jumpDistance, layerMask);
        if (hit.collider && !hit.collider.isTrigger)
        {
            //if (ladderManager != null)
            //playerAnimator.SetTrigger("jumpTrigger");
            return true;
        }
		return false;
	}

	void FixedUpdate()
	{

        if (!canMove) return;
        

        playerAnimator.SetFloat("walk_speed", Mathf.Abs(direction.x));
        float j = 0;
        //float j = (Input.GetKeyDown(jumpButton) && GetJump()) ? jumpForce : 0;
        if (Input.GetKey(jumpButton) && GetJump())
        {
            playerAnimator.SetTrigger("jumpTrigger");
            //body.AddForce(Vector2.up * jumpForce);
            j = jumpForce;
        }
        direction.y = j;
        body.AddForce(direction * body.mass * speed * acceleration);
        if (Mathf.Abs(body.velocity.x) > speed)
		{
			body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed, body.velocity.y);
		}
        playerAnimator.SetBool("grounded", GetJump());
        playerAnimator.SetFloat("y_speed", body.velocity.y);
    }

	void Flip() // отражение по горизонтали
	{
        if (playerAnimator.GetBool("isOnLadder")) return;
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, Vector3.down * jumpDistance);
	}

	void Update () 
	{
        //if (Input.GetKeyDown(jumpButton) && GetJump())
        //{
        //    body.velocity = new Vector2(0, jumpForce);
        //}
        
        float h = Input.GetAxis("Horizontal");
        //float j = 0;
        ////float j = (Input.GetKeyDown(jumpButton) && GetJump()) ? jumpForce : 0;
        //if (Input.GetKey(jumpButton) && GetJump())
        //{
        //    playerAnimator.SetTrigger("jumpTrigger");
        //    //body.AddForce(Vector2.up * jumpForce);
        //    j = jumpForce;
        //}
        //playerAnimator.SetBool("jumpBool", Input.GetKeyDown(jumpButton) && GetJump());


        direction = new Vector2(h, 0);
        
		if(h > 0 && !facingRight) Flip(); else if(h < 0 && facingRight) Flip();
	}
}
