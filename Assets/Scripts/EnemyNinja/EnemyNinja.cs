using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNinja : MonoBehaviour, IDamage
{
    private Animator animator;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private StateMachine stateMachine;

    [SerializeField]
    private Transform ninjaTransform;
    [SerializeField]
    private Transform cloneNinjaTransform;
    private Transform targetTransform;

    [Header("Hit(transform)")]
    public Vector2 hitPosition;

    [Header("Hit(range)")]
    [Range(0f, 5f)]
    public float hitRange;

    [Header("Kunai(transform)")]
    public Vector2 kunaiThrowPosition;

    [Header("Enemy(layer)")]
    public LayerMask enemyLayer;

    [Header("Run(speed)")]
    public float runSpeed;

    [Header("Attack CD(s)")]
    public float attackCD;
    [Header("Throw CD(s)")]
    public float throwCD;
    [Header("Clone Attack CD(s)")]
    public float cloneAttackCD;
    [Header("Clone Throw CD(s)")]
    public float cloneThrowCD;
    [Header("Damage(melee)")]
    public float meleeDamage;
    [Header("EnemyHP")]
    public float enemyHP;

    [Header("Health Bar")]
    public Slider healthBar;
    public Ninja ninja { get; private set; }

    public int facingDirection { get; private set; }    
    public int knockDirection { get; private set; }    
    public bool canAttack { get; private set; }
    public bool canCloneAttack { get; private set; }
    public bool canThrow { get; private set; }
    public bool canCloneThrow { get; private set; }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ninja = ninjaTransform.GetComponent<Ninja>();

        targetTransform = ninjaTransform;

        stateMachine = new StateMachine();
        stateMachine.SetState(new EnemyIdle(this, stateMachine));

        facingDirection = 1;
        canAttack = true;
        canThrow = true;
        Invoke(nameof(CloneAttackCoolDown), cloneAttackCD);
        Invoke(nameof(CloneThrowCoolDown), cloneThrowCD);

        healthBar.minValue = 0;
        healthBar.maxValue = enemyHP;
        healthBar.value = enemyHP;
    }
    private void Update()
    {
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    private Vector2 GetColliderPosition(Vector2 offset)
    {
        return (Vector2)transform.position + new Vector2(offset.x * facingDirection, offset.y);
    }
    private void ThrowKunai()
    {
        GameObject kunai = KunaiPool.instance.GetFromPool();
        kunai.transform.position = GetColliderPosition(kunaiThrowPosition);
        kunai.GetComponent<Kunai>().SetKunai((ninja.transform.position - transform.position), enemyLayer);
    }
    private void HitBox()
    {
        Collider2D hit = Physics2D.OverlapCircle(GetColliderPosition(hitPosition), hitRange, enemyLayer);
        if (hit)
        {
            int damageRange = (int)(meleeDamage / 10.0f);
            int damage = (int)Random.Range(meleeDamage - damageRange, meleeDamage + damageRange);
            hit.GetComponent<IDamage>().Damage(damage, transform.position.x, false);
        }
    }
    private void FinishState()
    {
        if (stateMachine.currentState is EnemyAttack)
        {
            canAttack = false;
            Invoke(nameof(AttackCoolDown), attackCD);
        }
        if (stateMachine.currentState is EnemyThrow)
        {
            canThrow = false;
            Invoke(nameof(ThrowCoolDown), throwCD);
        }
        stateMachine.SetState(new EnemyIdle(this, stateMachine));
    }
    private void AttackCoolDown()
    {
        canAttack = true;
    }
    private void CloneAttackCoolDown()
    {
        canCloneAttack = true;
    }
    private void ThrowCoolDown()
    {
        canThrow = true;
    }
    private void CloneThrowCoolDown()
    {
        canCloneThrow = true;
    }
    public void CloneAttackReset()
    {
        canCloneAttack = false;
        Invoke(nameof(CloneAttackCoolDown), cloneAttackCD);
    }
    public void CloneThrowReset()
    {
        canCloneThrow = false;
        Invoke(nameof(CloneThrowCoolDown), cloneThrowCD);
    }
    public void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        facingDirection *= -1;
    }
    public void SetAnimation(int hash, bool flag)
    {
        animator.SetBool(hash, flag);
    }
    public void Damage(float damage, float xPos, bool crit)
    {
        HitPool.instance.GetFromPool().transform.position = transform.position;

        GameObject damageText = DamageTextPool.instance.GetFromPool();
        damageText.transform.position = transform.position;
        float damaged = crit ? damage * 2 : damage;
        Color color = crit ? Color.red : Color.yellow;
        int fontSize = crit ? 13 : 12;
        int directon = xPos > transform.position.x ? -1 : 1;
        knockDirection = directon;
        damageText.GetComponent<DamageText>().SetText(damaged.ToString(), color, fontSize, directon);

        enemyHP -= damage;
        healthBar.value = Mathf.Clamp(enemyHP, healthBar.minValue, healthBar.maxValue);

        if (enemyHP > 0)
        {
            if (!canAttack && !canCloneAttack && !canCloneThrow)
                stateMachine.SetState(new EnemyHit(this, stateMachine));
        }
        else
        {
            Time.timeScale = 0.3f;
            stateMachine.SetState(new EnemyDead(this, stateMachine));
        }
    }
    public void SwitchTarget()
    {
        if (targetTransform == ninjaTransform)
        {
            targetTransform = cloneNinjaTransform;
        }
        else 
            targetTransform = ninjaTransform;
    }
    public Transform Target
    {
        get { return targetTransform; }
        set { targetTransform = value; }
    }
    public Rigidbody2D SetRb2D
    {
        get { return rb2D; }
        set { rb2D = value; }
    }
    private void End()
    {
        Time.timeScale = 0f;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + kunaiThrowPosition, 0.3f);
        Gizmos.DrawWireSphere((Vector2)transform.position + hitPosition, hitRange);
    }
}
