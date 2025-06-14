using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer ninjaSpriteRenderer;
    private Transform ninja;


    private float spriteAlpha;
    private float disappearTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ninja = GameObject.FindGameObjectWithTag("Player").transform;
        ninjaSpriteRenderer = ninja.transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ninjaSpriteRenderer.sprite;
    }
    private void OnEnable()
    {
        spriteRenderer.flipX = ninjaSpriteRenderer.GetComponent<Ninja>().facingDirection < 0 ? true : false;
        spriteAlpha = 2.0f;
        transform.position = ninja.position;
        transform.rotation = ninja.rotation;
        disappearTimer = Time.time;
    }
    private void Update()
    {
        spriteAlpha *= 0.85f;
        spriteRenderer.color = new Color(255, 255, 255, spriteAlpha);
        if (Time.time >= disappearTimer + 0.3f)
            GhostPool.instance.AddToPool(gameObject);
    }
}
