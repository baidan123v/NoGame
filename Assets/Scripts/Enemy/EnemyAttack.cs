using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeleeEnemy))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackPower = 10f;

    [SerializeField] private Transform attackPoint;
    private Animator animator;
    private MeleeEnemy enemyController;


    void Start()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<MeleeEnemy>();
    }
    

    void Update()
    {
        if (enemyController.state == EnemyState.Waiting)
        {
            GameObject playerOnAttackPoint = CheckPlayerOnAttackPoint();
            if (playerOnAttackPoint != null)
            {
                StartAttack();
            }
        }
    }  


    public void StartAttack()
    {
        animator.SetTrigger("Attack");
        enemyController.DisableMovement();
        enemyController.SetState(EnemyState.Attacking);
    }


    public void OnAttackFrame()
    {
        GameObject playerOnAttackPoint = CheckPlayerOnAttackPoint();
        if (playerOnAttackPoint != null)
        {
            playerOnAttackPoint.GetComponent<CharacterSubController>().parentController.GetHit(attackPower, transform.position);
        }
    }

    
    public void OnAttackEnd()
    {
        // Attacking state was set on animation start, and if it was not changed to dead state during animation,
        // it gets reverted to waiting state
        if (enemyController.state == EnemyState.Attacking)
        {
            enemyController.SetState(EnemyState.Waiting);
            enemyController.EnableMovement();
        }
    }


    private GameObject CheckPlayerOnAttackPoint()
    {
        Collider2D[] collidersFront = Physics2D.OverlapCircleAll(attackPoint.position, 0.9f);
		for (int i = 0; i < collidersFront.Length; i++)
		{
			if (collidersFront[i].gameObject.tag == "Player")
			{
                return collidersFront[i].gameObject;
			}
		}
        return null;
    }
}
