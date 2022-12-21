using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterSubController : CharacterSubScontroller
{
    public PlayerMovement movement;

    public void RollEnd()
    {
        movement.OnRollEnd();
    }
}
