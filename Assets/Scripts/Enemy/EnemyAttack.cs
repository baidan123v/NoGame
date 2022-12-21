using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackPower = 10f;

    [SerializeField] private Transform attackPoint;
    private Animator animator;
    private EnemyAttackState attackState = EnemyAttackState.Waiting;


    void Start()
    {
        animator = GetComponent<Animator>();
    }
    

    void Update()
    {
        if (attackState == EnemyAttackState.Waiting)
        {
            GameObject playerOnAttackPoint = CheckPlayerOnAttackPoint();
            if (playerOnAttackPoint != null)
            {
                StartAttack();
            }
        }
    }


    public void SetAttackState(EnemyAttackState newState)
    {
        attackState = newState;
    }


    public void StartAttack()
    {
        animator.SetTrigger("Attack");
        SetAttackState(EnemyAttackState.Attacking);
    }


    public void OnAttackFrame()
    {
        GameObject playerOnAttackPoint = CheckPlayerOnAttackPoint();
        if (playerOnAttackPoint != null)
        {
            playerOnAttackPoint.GetComponent<CharacterSubScontroller>().parentController.GetHit(attackPower, transform.position);
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
