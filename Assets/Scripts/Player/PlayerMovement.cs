using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public PlayerInput input;

	private Rigidbody2D rb2d;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public float jumpForce = 400f;
	[Range(0, .3f)] public float movementSmoothing = .05f;
	public bool enableAirControl = true;
	public LayerMask groundLayer;
	public Transform groundCheck;

	private PlayerMovementState state = PlayerMovementState.OnGround;
	private bool canMove = true;
	private float limitFallSpeed = 25f;
	private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}


	private void Start()
	{
	}


	private void SetState(PlayerMovementState newState)
	{
		state = newState;
	}


	void Update ()
	{
		Move();
	}


	void FixedUpdate()
	{
		ApplyMovementLimits();
		CheckGround();
	}


	private void ApplyMovementLimits()
	{
		// Fall speed limit
		if (rb2d.velocity.y < -limitFallSpeed)
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, -limitFallSpeed);
		}
	}


	private void CheckGround()
	{
		if (state == PlayerMovementState.InAir && rb2d.velocity.y < 0) {
			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, k_GroundedRadius, groundLayer);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					Landing();
					return;
				}
			}
		}
	}


	private void Landing()
	{
		SetState(PlayerMovementState.OnGround);
		controller.currentAnimator.SetBool("IsJumping", false);
	}


	private void Move()
	{
		if (canMove)
		{	
			// Movement availability check
			if (state == PlayerMovementState.OnGround || state == PlayerMovementState.InAir && enableAirControl)
			{
				MoveHorizontal();
			}

			// Jump availability check
			if (state == PlayerMovementState.OnGround && controller.currentCharacterParams.canJump)
			{
				Jump();
			}

			// Roll availability check
			if (state == PlayerMovementState.OnGround && controller.currentCharacterParams.canRoll)
			{
				Roll();
			}
		}
	}


	private void MoveHorizontal()
	{
		controller.currentAnimator.SetFloat("Speed", Mathf.Abs(input.xMovement));

		Vector3 targetVelocity = new Vector2(input.xMovement * controller.currentCharacterParams.runSpeed, rb2d.velocity.y);
		// Smooth movement
		rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, movementSmoothing);
		// rb2d.velocity = targetVelocity;

		// Flip player direction based on input
		if (input.xMovement > 0 && !controller.isFacingRight)
		{
			// ... flip the player.
			controller.Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (input.xMovement < 0 && controller.isFacingRight)
		{
			// ... flip the player.
			controller.Flip();
		}
	}


	private void Jump()
	{
		if (input.jumpInput) {
			SetState(PlayerMovementState.InAir);

			controller.currentAnimator.SetBool("IsJumping", true);

			rb2d.AddForce(new Vector2(0f, jumpForce));
		}
	}


	private void Roll()
	{
		if (input.jumpInput)
		{
			SetState(PlayerMovementState.InRoll);

			float speedX = controller.currentCharacterParams.rollSpeed;
			if (!controller.isFacingRight)
			{
				speedX *= -1;
			}
			rb2d.velocity = new Vector2(speedX, 0);

			controller.currentAnimator.SetTrigger("Roll");
		}
	}


	public void OnRollEnd()
	{
		SetState(PlayerMovementState.OnGround);
	}


	public void OnFall()
	{
		controller.currentAnimator.SetBool("IsJumping", true);
	}


	public void OnLanding()
	{
		controller.currentAnimator.SetBool("IsJumping", false);
	}


	public void DisableMovement()
	{
		velocity = Vector3.zero;
		rb2d.velocity = Vector2.zero;
		controller.currentAnimator.SetFloat("Speed", 0);
		canMove = false;
	}


	public void EnableMovement() 
	{
		canMove = true;
	}
}

public enum PlayerMovementState {
	OnGround,
	InAir,
	InRoll
}
