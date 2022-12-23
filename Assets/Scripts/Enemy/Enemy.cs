using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ContactDamageController))]
[RequireComponent(typeof(HPController))]
public abstract class Enemy : MonoBehaviour {

	public float speed = 3f;
	public float collisionAttackPower = 15f;
	public EnemyState state {get; private set;} = EnemyState.Waiting;
	[SerializeField] protected bool isInvincible = false;

	private ContactDamageController contactDamageController;
	private HPController hpController;
	
	void Awake()
	{
		contactDamageController = GetComponent<ContactDamageController>();

		hpController = GetComponent<HPController>();
		hpController.DeathEvent.AddListener(Die);
	}


	public void SetState(EnemyState newState)
    {
        state = newState;
    }


	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && state != EnemyState.Dead)
		{
			collision.gameObject.GetComponent<ContactDamageController>().GetHit(collisionAttackPower, transform.position);
		}
	}

	
	public void Die()
	{
		SetState(EnemyState.Dead);
		StartCoroutine(DestroyEnemy());
	}

	public abstract IEnumerator DestroyEnemy();
}


public enum EnemyState {
    Waiting,
    Attacking,
	Dead
}
