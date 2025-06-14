using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;

    private TextMeshPro textMesh;
    private float gravityScale;
    private float lifeTimer;

    private Vector3 velocity;
    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        gravityScale = -4.9f;
    }
    private void OnEnable()
    {
        lifeTimer = Time.time;
    }
    private void Update()
    {
        velocity.y += gravityScale * Time.deltaTime;
        transform.position += velocity * speed * Time.deltaTime;

        if (Time.time >= lifeTimer + lifeTime)
        {
            DamageTextPool.instance.AddToPool(gameObject);
        }
    }
    public void SetText(string text, Color color, int size, int direction)
    {
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize = size;
        velocity = new Vector3(direction * Random.Range(0.35f, 0.55f), 1, 0).normalized;
    }
}
