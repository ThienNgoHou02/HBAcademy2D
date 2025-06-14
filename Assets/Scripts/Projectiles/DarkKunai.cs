using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DarkKunai : MonoBehaviour
{
    [Header("Speed")]
    public float speed;
    [Header("Size")]
    public Vector2 size;
    [Header("Layer")]
    public LayerMask layerCollision;

    private Vector2 _direction;
    private float angleCollision;
    private float appearTimer;
    private int appearAngle;
    private bool targetTouched;
    private Transform target;

    private void OnEnable()
    {
        appearTimer = Time.time;
        targetTouched = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void Update()
    {
        if (!targetTouched)
        {
            if (Time.time >= appearTimer + 0.5f)
            {
                _direction = target.position - transform.position;
                float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90.0f);
                angleCollision = angle - 90.0f;
            }
            Collider2D hit = Collision();
            if (hit)
            {
                int damage = (int)Random.Range(70, 80);
                hit.GetComponent<IDamage>().Damage(damage, transform.position.x, false);
                targetTouched = true;
                Invoke(nameof(DisableKunai), 1.0f);
            }
        }
        transform.position += new Vector3(_direction.x * speed * Time.deltaTime, _direction.y * speed * Time.deltaTime, 0);
    }
    private void DisableKunai()
    {
        DarkKunaiPool.instance.AddToPool(gameObject);
    }
    private Collider2D Collision()
    {
        return Physics2D.OverlapBox(transform.position, size, angleCollision, layerCollision);
    }
    public void SetTarget(Transform target, int appearAngle)
    {
        this.target = target;
        //this.appearAngle = appearAngle;
        float radianAngle = (appearAngle - 90.0f) * Mathf.Deg2Rad;
        transform.rotation = Quaternion.Euler(0, 0, appearAngle - 180.0f);
        _direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)).normalized;
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
