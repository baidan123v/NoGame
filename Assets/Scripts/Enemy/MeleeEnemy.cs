using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyAttack))]
public class MeleeEnemy : Enemy
{
    public Transform fallCheck;
	public Transform wallCheck;
	public LayerMask obstaclesMask;
	public LayerMask groundMask;
	[SerializeField] public bool canMove {get; private set;} = true;
	
    [SerializeField] private bool isFacingRight = false;


    private Rigidbody2D rb2d;
	private Animator animator;
	private EnemyAttack attack;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		attack = GetComponent<EnemyAttack>();
    }


    void FixedUpdate () {
		CheckEndOfSpace();
		// Non-zero vertical velocity means enemy was hit and shouldn't change speed
		if (rb2d.velocity.y == 0)
		{
			Move();
		}
		
	}


    void Move()
	{
		if (!canMove)
		{
			return;
		}

		float xSpeed = speed;
		if (!isFacingRight)
		{
			xSpeed *= -1;
		}

		rb2d.velocity = new Vector2(xSpeed, rb2d.velocity.y);
	}


	void CheckEndOfSpace()
	{
		bool isAtTheEdge = !(Physics2D.OverlapCircle(fallCheck.position, .2f, groundMask));
		bool isFacingObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, obstaclesMask);

		if ((isAtTheEdge || isFacingObstacle) && rb2d.velocity.y == 0)
		{
			Flip();
		}
	}


	void Flip ()
	{
		isFacingRight = !isFacingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


    override public IEnumerator DestroyEnemy()
	{
		canMove = false;
		rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}

	
	public void EnableMovement()
	{
		canMove = true;
	}


	public void DisableMovement()
	{
		canMove = false;
		rb2d.velocity = Vector2.zero;
	}
}
