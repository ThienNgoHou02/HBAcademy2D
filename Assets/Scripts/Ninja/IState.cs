using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}
public interface IDamage
{
    void Damage(float damage, float xPos, bool crit);
}