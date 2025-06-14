using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneNinja : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform ninja;
    private int facingDirection;

    [Header("Hit(transform)")]
    public Vector2 hitPosition;

    [Header("Hit(range)")]
    [Range(0f, 5f)]
    public float hitRange;

    [Header("Kunai(transform)")]
    public Vector2 kunaiThrowPosition;

    [SerializeField] private LayerMask enemyLayer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        facingDirection = 1;
    }
    private void OnEnable()
    {
        spriteRenderer.flipX = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private Vector2 GetColliderPosition(Vector2 offset)
    {
        return (Vector2)transform.position + new Vector2(transform.right.x * offset.x * facingDirection, offset.y);
    }
    private void HitBox()
    {
        Collider2D hit = Physics2D.OverlapCircle(GetColliderPosition(hitPosition), hitRange, enemyLayer);
        if (hit)
        {
            int damage = (int)Random.Range(68,  82);
            hit.GetComponent<IDamage>().Damage(damage, transform.position.x, false);
        }
    }
    public bool SetFlip
    {
        get { return spriteRenderer.flipX; }
        set { spriteRenderer.flipX = value; }
    }
    public void CloneNinjaAttack(float x)
    {
        animator.SetBool("isAttacking", true);
        if (transform.position.x > x && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            facingDirection = -1;
        }
        if (transform.position.x < x && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            facingDirection = 1;
        }
    }
    private void FinishAttack()
    {
        animator.SetBool("isAttacking", false);
        CloneNinjaPool.instance.AddToPool(gameObject);
    }
    public void CloneNinjaThrow(Transform ninja)
    {
        animator.SetBool("isThrowing", true);
        if (transform.position.x > ninja.position.x && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            facingDirection = -1;
        }
        if (transform.position.x < ninja.position.x && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            facingDirection = 1;
        }
        this.ninja = ninja;
    }
    private void FinishThrow()
    {
        animator.SetBool("isThrowing", false);
        CloneNinjaPool.instance.AddToPool(gameObject);
    }
    private void ThrowKunai()
    {
        GameObject kunai = KunaiPool.instance.GetFromPool();
        kunai.transform.position = GetColliderPosition(kunaiThrowPosition);
        kunai.GetComponent<Kunai>().SetKunai((ninja.position - transform.position), enemyLayer);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.right + new Vector2(transform.right.x * kunaiThrowPosition.x, kunaiThrowPosition.y), 0.3f);
        Gizmos.DrawWireSphere((Vector2)transform.position + hitPosition, hitRange);
    }
}
