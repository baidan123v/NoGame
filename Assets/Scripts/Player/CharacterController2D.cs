using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private GameObject jackObject;
	[SerializeField] private CharacterParams jackParams;
	[SerializeField] private GameObject peterObject;
	[SerializeField] private CharacterParams peterParams;

	private Rigidbody2D rb2d;
	private PlayerMovement movement;
	private PlayerInput input;
	public bool isFacingRight {get; private set;} = true;
	public Animator currentAnimator {get; private set;}
	public CharacterParams currentCharacterParams {get; private set;}

	public HealthBar healthBar;
	public bool isInvincible = false;
	public float maxLife = 10f;

	public float life {get; private set;}

	private CharacterState characterState = CharacterState.Jack;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		movement = GetComponent<PlayerMovement>();
		input = GetComponent<PlayerInput>();
		SetLife(maxLife);
		if (jackObject.activeSelf)
		{
			currentCharacterParams = jackParams;
			characterState = CharacterState.Jack;
			currentAnimator = jackObject.GetComponent<Animator>();
		}
		else
		{
			characterState = CharacterState.Peter;
			currentCharacterParams = peterParams;
			currentAnimator = peterObject.GetComponent<Animator>();
		}
		
	}


	void Update()
	{
		// Check for character change input
		if (input.switchCharacterInput)
		{
			SwitchCharacter();
		}
	}


	private void SetCharacterState(CharacterState newState)
	{
		characterState = newState;
	}


	private void SwitchCharacter()
	{
		if (characterState == CharacterState.Jack)
		{
			// If current character is jack, switch to peter
			jackObject.SetActive(false);
			peterObject.SetActive(true);
			SetCharacterState(CharacterState.Peter);
			currentCharacterParams = peterParams;
			currentAnimator = peterObject.GetComponent<Animator>();
		}
		else if (characterState == CharacterState.Peter)
		{
			// If current character is peter, switch to jack
			jackObject.SetActive(true);
			peterObject.SetActive(false);
			SetCharacterState(CharacterState.Jack);
			currentCharacterParams = jackParams;
			currentAnimator = jackObject.GetComponent<Animator>();
		}
	}


	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		isFacingRight = !isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	// ========================
	// Life/mana related
	public void RestoreLife(float amount)
	{
		float newLife = life + amount;
		if (newLife > maxLife)
		{
			newLife = maxLife;
		}
		SetLife(newLife);
	}


	public void SetLife(float newLife)
	{
		life = newLife;
		healthBar.SetHealth(newLife, maxLife);
	}
	// Life/mana related end
	// ========================


	// ========================
	// Damage-taking related
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
		if (isInvincible)
		{
			return;
		}

		currentAnimator.SetBool("Hit", true);

		life -= damage;
		SetLife(life - damage);

		if (life <= 0)
		{
			StartCoroutine(WaitToDead());
		}
		else
		{
			StartCoroutine(Stun(0.25f));
			StartCoroutine(MakeInvincible(1f));
		}
	}


	public void Knockback(Vector3 hitPosition, float knockbackMultiplier)
	{
		Vector2 damageDir = Vector3.Normalize(transform.position - hitPosition);
		damageDir.y = 0.4f;
		
		rb2d.velocity = Vector2.zero;
		rb2d.AddForce(damageDir * knockbackMultiplier);
	}


	public IEnumerator Stun(float time) 
	{
		movement.DisableMovement();
		yield return new WaitForSeconds(time);
		movement.EnableMovement();
	}


	IEnumerator MakeInvincible(float time) 
	{
		isInvincible = true;
		yield return new WaitForSeconds(time);
		isInvincible = false;
	}


	IEnumerator WaitToDead()
	{
		currentAnimator.SetBool("IsDead", true);
		movement.DisableMovement();
		isInvincible = true;

		yield return new WaitForSeconds(0.4f);
		rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	// Damage-taking related end
	// ========================

}

public enum CharacterState
{
	Jack,
	Peter
}