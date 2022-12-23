using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class HelicopterController : MonoBehaviour
{
    [SerializeField] private List<Transform> followPoints;
    [SerializeField] private float movementSpeed = 10f;

    private Rigidbody2D rb2d;
    private BoxCollider2D collider;
    private HelicopterState state = HelicopterState.Idle;
    private Animator animator;
    private int currentFollowIndex = -1;

    void Awake()
    {
        GetComponent<HPController>().DeathEvent.AddListener(Die);
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (state == HelicopterState.Flying)
        {
            CheckForReachedPoint();
        }
    }


    public void Activate()
    {
        SetState(HelicopterState.Flying);
        animator.SetBool("IsFlying", true);
        MoveToNextPoint();
    }


    private void SetState(HelicopterState newState)
    {
        state = newState;
    }


    void MoveToNextPoint()
    {
        currentFollowIndex++;
        if (currentFollowIndex >= followPoints.Count)
        {
            currentFollowIndex = 0;
        }

        Vector2 direction = GetPointDirection(followPoints[currentFollowIndex]);
        rb2d.velocity = direction * movementSpeed;
    }


    void CheckForReachedPoint()
    {
        if (collider.OverlapPoint(followPoints[currentFollowIndex].position))
        {
            MoveToNextPoint();
        }
    }


    Vector2 GetPointDirection(Transform point)
    {
        Vector2 direction = point.position - transform.position;
        direction.Normalize();
        return direction;
    }


    void Die()
    {
        GameObject.Destroy(gameObject);
    }
}


public enum HelicopterState
{
    Idle,
    Flying,
    Attacking
}
