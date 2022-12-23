using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(JackSubController))]
public class JackAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PlayerInput input;
    
    private CharacterController2D mainController;

    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        mainController = GetComponent<JackSubController>().parentController;
    }

    void Update()
    {
        if (input.attackInput)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void OnAttackFrame()
    {
        Collider2D[] collidersFront = Physics2D.OverlapCircleAll(attackPoint.position, 0.9f);
        for (int i = 0; i < collidersFront.Length; i++)
		{
			if (!collidersFront[i].isTrigger && collidersFront[i].gameObject.tag == "Enemy")
			{
                collidersFront[i].gameObject.GetComponent<ContactDamageController>().GetHit(mainController.currentCharacterParams.attackPower, transform.position);
                return;
			}
		}
    }
}
