using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyAttack))]
public class Enemy : MonoBehaviour {

	public float life = 10;
	public float speed = 3f;
	public Transform fallCheck;
	public Transform wallCheck;
	public LayerMask obstaclesMask;
	public LayerMask groundMask;
	[SerializeField] public bool canMove {get; private set;} = true;
	public EnemyState state {get; private set;} = EnemyState.Waiting;

	private Rigidbody2D rb2d;
	private Animator animator;
	private EnemyAttack attack;

	[SerializeField] private bool isFacingRight = false;
	[SerializeField] private bool isInvincible = false;
	

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		attack = GetComponent<EnemyAttack>();
	}


	void Update()
	{
		if (life <= 0) {
			Die();
		}
	}


	public void SetState(EnemyState newState)
    {
        state = newState;
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


	public void GetHit(float damage, Vector3 hitPosition)
	{
		if (!isInvincible)
		{
			ApplyDamage(damage);
			Knockback(hitPosition, 600f);
		}
	}


	public void ApplyDamage(float damage)
	{
		life -= damage;
		StartCoroutine(HitTime());
	}


	public void Knockback(Vector3 hitPosition, float knockbackMultiplier)
	{
		Vector2 damageDir = Vector3.Normalize(transform.position - hitPosition);
		damageDir.y = 0.4f;
		
		rb2d.velocity = Vector2.zero;
		rb2d.AddForce(damageDir * knockbackMultiplier);
	}


	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().GetHit(2f, transform.position);
			animator.SetTrigger("Attack");
		}
	}

	
	public void Die()
	{
		SetState(EnemyState.Dead);
		StartCoroutine(DestroyEnemy());
	}


	IEnumerator HitTime()
	{
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isInvincible = false;
	}


	IEnumerator DestroyEnemy()
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


public enum EnemyState {
    Waiting,
    Attacking,
	Dead
}
