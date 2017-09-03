using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    public float groundMoveForce = 365f;
	public float airMoveForce = 100f;
	float moveForce;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
	public float jumpForceAdditional = 500f;
	public float jumpTime;
	public float jumpTimeCounter;
	public bool stoppedJumping = true;
    public Transform groundCheck;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
		jumpTimeCounter = jumpTime;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
		moveForce = groundMoveForce;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if(grounded){
			jumpTimeCounter = jumpTime;
			moveForce = groundMoveForce;
		} else {
			moveForce = airMoveForce;
		}
		if(Input.GetButtonDown("Jump")){
			jump = true;
		}

		if(Input.GetButtonUp("Jump")){
			jump = false;
		}
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(h));

		if(Mathf.Abs(h)<0.05){
			anim.speed = 1.0f;
			rb2d.velocity = new Vector2(0,rb2d.velocity.y);
		} else {
			anim.speed = Mathf.Abs(h*3);
		}
		

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump && grounded && stoppedJumping)
        {
            anim.SetTrigger("Jump");
			Debug.Log("Adding Inital Force");
            rb2d.AddForce(new Vector2(0f, jumpForce));
			stoppedJumping = false;
        }
		if(!jump){
			jumpTimeCounter = 0;
			stoppedJumping = true;
		}

		if(jump && !stoppedJumping){
			if(jumpTimeCounter > 0){
				//Debug.Log("Adding Additional Force");
				rb2d.AddForce(new Vector2(0f,jumpForceAdditional));
				jumpTimeCounter -= Time.deltaTime;
			} else {
				jumpTimeCounter = 0;
				jump = false;
				stoppedJumping = true;
			}
		}

		
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
