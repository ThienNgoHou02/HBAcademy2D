using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    public Idle(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isIdling");   
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
    }
    public void Update()
    {
        if (playerInput.AttackInput())
            stateMachine.SetState(new Attack(ninja, stateMachine, playerInput));
        else if (playerInput.SlideInput())
            stateMachine.SetState(new Slide(ninja, stateMachine, playerInput));
        else if (playerInput.ThrowInput())
            stateMachine.SetState(new Throw(ninja, stateMachine, playerInput));
        else if (playerInput.SpecialKunaiInput())
            stateMachine.SetState(new SpecialKunai(ninja, stateMachine, playerInput));
        else if (playerInput.JumpInput() && ninja.isOnGround())
            stateMachine.SetState(new Jump(ninja, stateMachine, playerInput));
        else if (playerInput.HorizontalValue() != 0.0f)
            stateMachine.SetState(new Run(ninja, stateMachine, playerInput));

        if (playerInput.StealthInput())
            ninja.StartStealth();
    }
    public void FixedUpdate()
    {
        if (ninja.SetRb2D.velocity.x != 0)
            ninja.SetRb2D.velocity = new Vector2(0, ninja.SetRb2D.velocity.y);
    }
    public void Exit() 
    { 
        ninja.SetAnimation(hash, false);
    }
}
public class Run : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    public Run(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isRunning");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
    }
    public void Update()
    {
        if (playerInput.AttackInput())
            stateMachine.SetState(new Attack(ninja, stateMachine, playerInput));
        else if (playerInput.SlideInput())
            stateMachine.SetState(new Slide(ninja, stateMachine, playerInput));
        else if (playerInput.ThrowInput())
            stateMachine.SetState(new Throw(ninja, stateMachine, playerInput));
        else if (playerInput.SpecialKunaiInput())
            stateMachine.SetState(new SpecialKunai(ninja, stateMachine, playerInput));
        else if (playerInput.JumpInput() && ninja.isOnGround())
            stateMachine.SetState(new Jump(ninja, stateMachine, playerInput));
        else if (playerInput.HorizontalValue() == 0.0f)
            stateMachine.SetState(new Idle(ninja, stateMachine, playerInput));

        if (playerInput.StealthInput())
            ninja.StartStealth();
    }
    public void FixedUpdate()
    {
        ninja.SetRb2D.velocity = new Vector2(playerInput.HorizontalValue() * ninja.runSpeed * Time.deltaTime, ninja.SetRb2D.velocity.y);
    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}
public class Jump : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    public Jump(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isJumping");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
        ninja.SetRb2D.velocity = new Vector2(ninja.SetRb2D.velocity.x, ninja.jumpForce);
    }
    public void Update()
    {
        if (ninja.isOnGround() && ninja.SetRb2D.velocity.y < 0)
        {
            stateMachine.SetState(new Idle(ninja, stateMachine, playerInput));
        }
            
    }
    public void FixedUpdate()
    {

    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}
public class Attack : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    private readonly int comboHash;
    private float xPos;
    public Attack(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isAttacking");
        comboHash = Animator.StringToHash("Combo");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
        if ((ninja.Combo > 0 && Time.time >= (ninja.ComboTimer + 0.5f)) || ninja.Combo >= 3)
            ninja.Combo = 0;

        ninja.SetAnimator.SetInteger(comboHash, ninja.Combo);
        xPos = ninja.transform.position.x;
    }
    public void Update()
    {
    
    }
    public void FixedUpdate()
    {
        if (Mathf.Abs(ninja.transform.position.x - xPos) < 1.0f)
            ninja.SetRb2D.velocity = new Vector2(ninja.facingDirection * 100 * Time.deltaTime, ninja.SetRb2D.velocity.y);
    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}
public class Throw : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    public Throw(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isThrowing");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}
public class Slide : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    private float xPos;
    public Slide(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isSliding");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
        xPos = ninja.transform.position.x;
    }
    public void Update()
    {
        ninja.SetRb2D.velocity = new Vector2(ninja.facingDirection * ninja.slideSpeed * Time.deltaTime, ninja.SetRb2D.velocity.y);
        if (Mathf.Abs(ninja.transform.position.x - xPos) >= 0.25f)
        {
            GhostPool.instance.GetFromPool();
            xPos = ninja.transform.position.x;
        }
    }
    public void FixedUpdate()
    {

    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}
public class Hitting : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    public Hitting(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isHitting");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
        ninja.SetRb2D.velocity = new Vector2(ninja.knockDirection * 3.0f, 3.0f);
    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}
public class SpecialKunai : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    public SpecialKunai(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isIdling");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
    }
    public void Update()
    {
        for (int i = 36; i < 180; i+=36)
        {
            int startAngle = i * -ninja.facingDirection;
            GameObject darkKunai = DarkKunaiPool.instance.GetFromPool();
            darkKunai.transform.position = ninja.transform.position;
            darkKunai.GetComponent<DarkKunai>().SetTarget(ninja.enemyTransform, startAngle);
        }
        stateMachine.SetState(new Idle(ninja, stateMachine, playerInput));
    }
    public void FixedUpdate()
    {

    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}
public class Dead : IState
{
    private readonly Ninja ninja;
    private readonly StateMachine stateMachine;
    private readonly PlayerInput playerInput;
    private readonly int hash;
    public Dead(Ninja ninja, StateMachine stateMachine, PlayerInput playerInput)
    {
        this.ninja = ninja;
        this.stateMachine = stateMachine;
        this.playerInput = playerInput;
        hash = Animator.StringToHash("isDead");
    }
    public void Enter()
    {
        ninja.SetAnimation(hash, true);
        ninja.gameObject.layer = 8;
    }
    public void Update()
    {
        
    }
    public void FixedUpdate()
    {

    }
    public void Exit() 
    {
        ninja.SetAnimation(hash, false);
    }
}


