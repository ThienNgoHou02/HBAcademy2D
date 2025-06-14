using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    public static DamageTextPool instance { get; private set; }
    [Header("TextMesh(GameObject)")]
    public GameObject textMesh;

    private Queue<GameObject> damageTextPool = new Queue<GameObject>();
    private void Awake()
    {
        instance = this;
    }
    private void CreatePoolComponent()
    {
        GameObject gameObject = Instantiate(textMesh);
        gameObject.transform.SetParent(transform);
        AddToPool(gameObject);
    }
    public void AddToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        damageTextPool.Enqueue(gameObject);
    }
    public GameObject GetFromPool()
    {
        if (damageTextPool.Count <= 0)
            CreatePoolComponent();

        GameObject gameObject = damageTextPool.Dequeue();
        gameObject.SetActive(true);
        return gameObject;
    }
}
