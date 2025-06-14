using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CloneInfor
{
    public bool flipX;
    public Vector2 position;
    public float zRotation;

    public CloneInfor(bool flipX, Vector2 position, float zRotation)
    {
        this.flipX = flipX;
        this.position = position;
        this.zRotation = zRotation;
    }
}
public class EnemyIdle : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;

    public EnemyIdle(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isIdling");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
        eNinja.SetRb2D.velocity = new Vector2(0, eNinja.SetRb2D.velocity.y);
    }
    public void Update()
    {
        float distance = Mathf.Abs(eNinja.Target.position.x - eNinja.transform.position.x);
        if (eNinja.canCloneThrow)
            stateMachine.SetState(new EnemyCloneThrow(eNinja, stateMachine));
        else if (eNinja.canCloneAttack)
            stateMachine.SetState(new EnemyCloneAttack(eNinja, stateMachine));
        else if (distance > 1.5f)
            stateMachine.SetState(new EnemyRun(eNinja, stateMachine));
        else if (distance > 5.0f && eNinja.canThrow)
            stateMachine.SetState(new EnemyThrow(eNinja, stateMachine));
        else
        {
            if (eNinja.canAttack)
                stateMachine.SetState(new EnemyAttack(eNinja, stateMachine));
        }
    }
    public void FixedUpdate()
    {

    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
public class EnemyRun : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;

    public EnemyRun(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isRunning");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
    }
    public void Update()
    {
        if (eNinja.facingDirection * eNinja.SetRb2D.velocity.x < 0)
        {
            eNinja.Flip();
        }
        float distance = Mathf.Abs(eNinja.Target.position.x - eNinja.transform.position.x);
        if (eNinja.canCloneThrow)
            stateMachine.SetState(new EnemyCloneThrow(eNinja, stateMachine));
        else if (eNinja.canCloneAttack)
            stateMachine.SetState(new EnemyCloneAttack(eNinja, stateMachine));
        else if (distance <= 1.5f)
        {
            if (eNinja.canAttack)
                stateMachine.SetState(new EnemyAttack(eNinja, stateMachine));
            else
                stateMachine.SetState(new EnemyIdle(eNinja, stateMachine));
        }
        else if (distance > 5.0f && eNinja.canThrow)
            stateMachine.SetState(new EnemyThrow(eNinja, stateMachine));
    }
    public void FixedUpdate()
    {
        int runDirection = eNinja.Target.position.x > eNinja.transform.position.x ? 1 : -1;
        eNinja.SetRb2D.velocity = new Vector2(runDirection * eNinja.runSpeed * Time.deltaTime, eNinja.SetRb2D.velocity.y);
    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
public class EnemyAttack : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;

    public EnemyAttack(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isAttacking");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
        eNinja.SetRb2D.velocity = new Vector2(0, eNinja.SetRb2D.velocity.y);
        if (eNinja.facingDirection * (eNinja.Target.position.x - eNinja.transform.position.x) < 0)
            eNinja.Flip();
    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
public class EnemyThrow : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;

    public EnemyThrow(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isThrowing");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
        eNinja.SetRb2D.velocity = new Vector2(0, eNinja.SetRb2D.velocity.y);
        if (eNinja.facingDirection * (eNinja.Target.position.x - eNinja.transform.position.x) < 0)
            eNinja.Flip();
    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
public class EnemyCloneAttack : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;
    private int spawnedCount = 0;
    private float spawnTimer;

    public EnemyCloneAttack(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isIdling");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
        spawnTimer = Time.time;
        eNinja.SetRb2D.velocity = Vector2.zero;
    }
    public void Update()
    {
        if (spawnedCount <= 5)
        {
            if (Time.time >= spawnTimer + 1.0f)
            {
                GameObject clone = CloneNinjaPool.instance.GetFromPool();
                clone.transform.position = new Vector2(eNinja.Target.position.x + (eNinja.ninja.facingDirection * 1.5f), eNinja.transform.position.y);
                clone.GetComponent<CloneNinja>().CloneNinjaAttack(eNinja.Target.position.x);
                spawnedCount++;
                spawnTimer = Time.time;
            }
        }
        else
        {
            stateMachine.SetState(new EnemyIdle(eNinja, stateMachine));
            eNinja.CloneAttackReset();
        }
    }
    public void FixedUpdate()
    {

    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
public class EnemyCloneThrow : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;
    private CloneInfor[] clones;

    public EnemyCloneThrow(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isIdling");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
        eNinja.SetRb2D.velocity = Vector2.zero;
        clones = new CloneInfor[]
        {
            new CloneInfor(true, new Vector2(eNinja.Target.position.x + 5f, eNinja.transform.position.y), 0f),
            new CloneInfor(true, new Vector2(eNinja.Target.position.x + 5f, eNinja.Target.position.y + 5f), 45f),
            new CloneInfor(false, new Vector2(eNinja.Target.position.x - 5f, eNinja.Target.position.y + 5f), -45f),
            new CloneInfor(false, new Vector2(eNinja.Target.position.x - 5f, eNinja.transform.position.y), 0f),
            new CloneInfor(false, new Vector2(eNinja.Target.position.x - 5f, eNinja.Target.position.y - 5f), 45f),
            new CloneInfor(true, new Vector2(eNinja.Target.position.x + 5f, eNinja.Target.position.y - 5f), -45f)
                    
        };
    }
    public void Update()
    {
        for (int i = 0; i < clones.Length; i++)
        {
            GameObject clone = CloneNinjaPool.instance.GetFromPool();
            clone.transform.position = clones[i].position;
            Vector3 rotate = new Vector3(0, 0, clones[i].zRotation);
            clone.transform.rotation = Quaternion.Euler(rotate);
            clone.GetComponent<CloneNinja>().SetFlip = clones[i].flipX;
            clone.GetComponent<CloneNinja>().CloneNinjaThrow(eNinja.Target);
        }
        eNinja.CloneThrowReset();
        stateMachine.SetState(new EnemyIdle(eNinja, stateMachine));
    }
    public void FixedUpdate()
    {

    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
public class EnemyHit : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;

    public EnemyHit(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isHitting");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
        eNinja.SetRb2D.velocity = new Vector2(3f * eNinja.knockDirection, 3f);
    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
public class EnemyDead : IState
{
    private readonly EnemyNinja eNinja;
    private readonly StateMachine stateMachine;
    private readonly int hash;

    public EnemyDead(EnemyNinja eNinja, StateMachine stateMachine)
    {
        this.eNinja = eNinja;
        this.stateMachine = stateMachine;
        hash = Animator.StringToHash("isDead");
    }
    public void Enter()
    {
        eNinja.SetAnimation(hash, true);
        eNinja.gameObject.layer = 8;
    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void Exit()
    {
        eNinja.SetAnimation(hash, false);
    }
}
