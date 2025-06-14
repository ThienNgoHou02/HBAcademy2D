using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPool : MonoBehaviour
{
    public static HitPool instance { get; private set; }
    [Header("Hit(GameObject)")]
    public GameObject hitGameObject;

    private Queue<GameObject> hitPool = new Queue<GameObject>();
    private void Awake()
    {
        instance = this;
    }
    private void CreatePoolComponent()
    {
        GameObject gameObject = Instantiate(hitGameObject);
        gameObject.transform.parent = transform;
        AddToPool(gameObject);
    }
    public void AddToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        hitPool.Enqueue(gameObject);
    }
    public GameObject GetFromPool()
    {
        if (hitPool.Count <= 0)
            CreatePoolComponent();

        GameObject gameObject = hitPool.Dequeue();
        gameObject.SetActive(true);
        return gameObject;
    }
}
