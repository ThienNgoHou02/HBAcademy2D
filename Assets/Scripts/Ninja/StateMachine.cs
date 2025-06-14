using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState currentState;

    public void SetState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    public void Update()
    {
        currentState?.Update();
    }
    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
}
