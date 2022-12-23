using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ContactDamageController))]
[RequireComponent(typeof(HPController))]
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private GameObject jackObject;
	[SerializeField] private CharacterParams jackParams;
	[SerializeField] private GameObject peterObject;
	[SerializeField] private CharacterParams peterParams;
	[SerializeField] private float damageStunDuration;
	[SerializeField] private float deathDelay;

	private Rigidbody2D rb2d;
	private PlayerMovement movement;
	private PlayerInput input;
	public bool isFacingRight {get; private set;} = true;
	public Animator currentAnimator {get; private set;}
	public CharacterParams currentCharacterParams {get; private set;}
	public HPController hpController {get; private set;}
	public ContactDamageController contactDamageController {get; private set;}

	private CharacterState characterState = CharacterState.Jack;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		movement = GetComponent<PlayerMovement>();
		input = GetComponent<PlayerInput>();
		
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
		
		contactDamageController = GetComponent<ContactDamageController>();
		contactDamageController.HitEvent.AddListener(StunOnDamage);

		hpController = GetComponent<HPController>();
		hpController.DeathEvent.AddListener(Die);
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
		movement.SetState(PlayerMovementState.OnGround);
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
	// Damage-taking related
	void StunOnDamage()
	{
		Stun(damageStunDuration);
	}


	public IEnumerator Stun(float time) 
	{
		movement.DisableMovement();
		yield return new WaitForSeconds(time);
		movement.EnableMovement();
	}


	void Die()
	{
		StartCoroutine(WaitToDead());
	}


	IEnumerator WaitToDead()
	{
		currentAnimator.SetBool("IsDead", true);
		movement.DisableMovement();
		rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		yield return new WaitForSeconds(deathDelay);
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