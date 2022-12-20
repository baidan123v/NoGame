using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float xMovement {get; private set;}
    public bool jumpInput {get; private set;}
    public bool meleeAttackInput {get; private set;}
    public bool rangeAttackInput {get; private set;}
    
    private KeyCode moveLeftKey = KeyCode.LeftArrow;
    private KeyCode moveRightKey = KeyCode.RightArrow;
    private KeyCode jumpKey = KeyCode.UpArrow;
    private KeyCode meleeAttackKey = KeyCode.J;
    private KeyCode rangeAttackKey = KeyCode.K;

    void Update()
    {
        if (Input.GetKey(moveRightKey) && !Input.GetKey(moveLeftKey))
        {
            xMovement = 1;
        }
        else if (Input.GetKey(moveLeftKey) && !Input.GetKey(moveRightKey))
        {
            xMovement = -1;
        }
        else
        {
            xMovement = 0;
        }

        jumpInput = Input.GetKeyDown(jumpKey);
        meleeAttackInput = Input.GetKeyDown(meleeAttackKey);
        rangeAttackInput = Input.GetKeyDown(rangeAttackKey);
    }


    public void alterControls()
    {
        moveLeftKey = KeyCode.W;
        moveRightKey = KeyCode.X;
        jumpKey = KeyCode.S;
        meleeAttackKey = KeyCode.O;
        rangeAttackKey = KeyCode.L;
    }


    public void restoreControls()
    {
        moveLeftKey = KeyCode.A;
        moveRightKey = KeyCode.D;
        jumpKey = KeyCode.W;
        meleeAttackKey = KeyCode.J;
        rangeAttackKey = KeyCode.K;
    }
}
