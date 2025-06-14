using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ninja : MonoBehaviour, IDamage
{
    private Rigidbody2D rb2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private StateMachine stateMachine;
    private PlayerInput playerInput;

    public int facingDirection { get; private set; }
    public int knockDirection { get; private set; }

    [Header("Enemy(transform)")]
    public Transform enemyTransform;
    
    [Header("Run(speed)")]
    public float runSpeed;

    [Header("Jump(force)")]
    public float jumpForce;

    [Header("Slide(speed)")]
    public float slideSpeed;

    [Header("Groundcheck(transform)")]
    public Vector2 groundCheckPosition;

    [Header("Groundcheck(layer)")]
    public LayerMask groundLayer;

    [Header("Groundcheck(radius)")]
    [Range(0f, 1f)]
    public float groundCheckRadius;

    [Header("Kunai(transform)")]
    public Vector2 kunaiThrowPosition;

    [Header("Enemy(layer)")]
    public LayerMask enemyLayer;

    [Header("Hit(transform)")]
    public Vector2 hitPosition;

    [Header("Hit(range)")]
    [Range(0f, 5f)]
    public float hitRange;

    [Header("Damage(attack)")]
    public float ninjaMeleeDamage;

    [Header("HP")]
    public float ninjaHP;

    [Header("Clone(transform)")]
    public Transform cloneTransform;

    [Header("Health Bar")]
    public Slider healthBar;

    private int comboIndex;
    private float nextComboTimer;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine = new StateMachine();
        playerInput = new PlayerInput();
        stateMachine.SetState(new Idle(this, stateMachine, playerInput));

        facingDirection = 1;
        comboIndex = 0;

        healthBar.minValue = 0;
        healthBar.maxValue = ninjaHP;
        healthBar.value = ninjaHP;
    }
    private void Update()
    {
        stateMachine.Update();
        Flip();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    private void Flip()
    {
        if (playerInput.HorizontalValue() < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            facingDirection *= -1;
        }
        if (playerInput.HorizontalValue() > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            facingDirection *= -1;
        }
    }
    private Vector2 GetColliderPosition(Vector2 offset)
    {
        return (Vector2)transform.position + new Vector2(offset.x * facingDirection, offset.y);
    }
    private void HitBox()
    {
        Collider2D hit = Physics2D.OverlapCircle(GetColliderPosition(hitPosition), hitRange, enemyLayer);
        if (hit)
        {
            int damageRange = (int)(ninjaMeleeDamage / 10.0f);
            int damage = (int)Random.Range(ninjaMeleeDamage - damageRange, ninjaMeleeDamage + damageRange);
            bool crit = Random.Range(0f, 1f) <= 0.3f;
            hit.GetComponent<IDamage>().Damage(damage, transform.position.x, crit);
            CameraController.instance.Shake(.3f);
        }
    }
    private void FinishState()
    {
        if (stateMachine.currentState is Attack)
        {
            comboIndex++;
            nextComboTimer = Time.time;
        }
        stateMachine.SetState(new Idle(this, stateMachine, playerInput));
    }
    private void ThrowKunai()
    {
        GameObject kunai = KunaiPool.instance.GetFromPool();
        kunai.transform.position = GetColliderPosition(kunaiThrowPosition);
        kunai.GetComponent<Kunai>().SetKunai((transform.right * facingDirection), enemyLayer);
    }
    private void End()
    {
        Time.timeScale = 0f;
    }
    public void Damage(float damage, float xPos, bool crit)
    {
        HitPool.instance.GetFromPool().transform.position = transform.position;

        GameObject damageText = DamageTextPool.instance.GetFromPool();
        damageText.transform.position = transform.position;
        int directon = xPos > transform.position.x ? -1 : 1;
        knockDirection = directon;
        damageText.GetComponent<DamageText>().SetText($"-{damage}", Color.yellow, 12, directon);

        ninjaHP -= damage;
        healthBar.value = Mathf.Clamp(ninjaHP, healthBar.minValue, healthBar.maxValue);
        if (ninjaHP > 0)
        {
            stateMachine.SetState(new Hitting(this, stateMachine, playerInput));
        }
        else
        {
            Time.timeScale = 0.3f;
            stateMachine.SetState(new Dead(this, stateMachine, playerInput));
        }
    }
    public void SetAnimation(int animationHash, bool flag)
    {
        animator.SetBool(animationHash, flag);
    }
    public bool isOnGround()
    {
        return Physics2D.OverlapCircle(GetColliderPosition(groundCheckPosition), groundCheckRadius, groundLayer);
    }
    public Rigidbody2D SetRb2D
    {
        get { return rb2D; }
        set { rb2D = value; }
    }
    public Animator SetAnimator
    {
        get { return animator; }
        set { animator = value; }
    }
    public int Combo
    {
        get { return comboIndex; }
        set { comboIndex = value; }
    }
    public float ComboTimer
    {
        get { return nextComboTimer; }
        set { nextComboTimer = value; }
    }
    public void StartStealth()
    {
        cloneTransform.gameObject.SetActive(true);
        cloneTransform.position = new Vector2(transform.position.x, cloneTransform.position.y);
        enemyTransform.GetComponent<EnemyNinja>().SwitchTarget();
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
        gameObject.layer = 8;
        Invoke(nameof(FinishStealth), 5);
    }
    public void FinishStealth()
    {
        cloneTransform.gameObject.SetActive(false);
        Color color = spriteRenderer.color;
        color.a = 1.0f;
        spriteRenderer.color = color;
        gameObject.layer = 3;
        enemyTransform.GetComponent<EnemyNinja>().SwitchTarget();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckPosition, groundCheckRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + kunaiThrowPosition, groundCheckRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + hitPosition, hitRange);
    }
}
