using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{

    [Header("Speed")]
    public float speed;

    [Header("Size")]
    public Vector2 size;

    private float angleCollision;
    private float kunaiDamage;
    private LayerMask layerCollision;
    private float destroyTimer;

    private Vector2 _direction;

    private void Awake()
    {
        kunaiDamage = 50;
    }
    private void Update()
    {
        transform.position += new Vector3(_direction.x * speed * Time.deltaTime, _direction.y * speed * Time.deltaTime, 0);
        Collider2D hit = Collision();
        if (!hit && Time.time >= (destroyTimer + 3.5f))
            KunaiPool.instance.AddToPool(gameObject);
        else if (hit)
        {
            int damageRange = (int)(kunaiDamage / 10.0f);
            int damage = (int)Random.Range(kunaiDamage - damageRange, kunaiDamage + damageRange);
            hit.GetComponent<IDamage>().Damage(damage, transform.position.x, false);
            KunaiPool.instance.AddToPool(gameObject);
        }
    }
    private Collider2D Collision()
    {
        return Physics2D.OverlapBox(transform.position, size, angleCollision, layerCollision);
    }
    public void SetKunai(Vector2 moveDirection, LayerMask layer)
    {
        _direction = moveDirection.normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90.0f);
        angleCollision = angle - 90.0f;
        layerCollision = layer;
        destroyTimer = Time.time;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(transform.position.x - size.x, transform.position.y - size.y), 
            new Vector2(transform.position.x + size.x, transform.position.y - size.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + size.x, transform.position.y - size.y), 
            new Vector2(transform.position.x + size.x, transform.position.y + size.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + size.x, transform.position.y + size.y), 
            new Vector2(transform.position.x - size.x, transform.position.y + size.y));
        Gizmos.DrawLine(new Vector2(transform.position.x - size.x, transform.position.y + size.y), 
            new Vector2(transform.position.x - size.x, transform.position.y - size.y));
    }
}
