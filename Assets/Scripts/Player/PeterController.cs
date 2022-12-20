using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterController : MonoBehaviour
{
    public PlayerMovement movement;

    public void RollEnd()
    {
        movement.OnRollEnd();
    }
}
