using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    public float HorizontalValue()
    {
        return Input.GetAxisRaw("Horizontal");
    }
    public bool JumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    public bool AttackInput()
    {
        return Input.GetKeyUp(KeyCode.J);
    }
    public bool ThrowInput()
    {
        return Input.GetKeyDown(KeyCode.K);
    }
    public bool SlideInput()
    {
        return Input.GetKeyUp(KeyCode.L);
    }
    public bool SpecialKunaiInput()
    {
        return Input.GetKeyUp(KeyCode.U);
    }
    public bool StealthInput()
    {
        return Input.GetKeyUp(KeyCode.I);
    }
}
