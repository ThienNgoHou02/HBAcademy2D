using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKunaiPool : MonoBehaviour
{
    public static DarkKunaiPool instance { get; private set; }
    [Header("Kunai(GameObject)")]
    public GameObject kunaiGameObject;

    private Queue<GameObject> kunaiPool = new Queue<GameObject>();
    private void Awake()
    {
        instance = this;
    }
    private void CreatePoolComponent()
    {
        GameObject gameObject = Instantiate(kunaiGameObject);
        gameObject.transform.parent = transform;
        AddToPool(gameObject);
    }
    public void AddToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        kunaiPool.Enqueue(gameObject);
    }
    public GameObject GetFromPool()
    {
        if (kunaiPool.Count <= 0)
            CreatePoolComponent();

        GameObject gameObject = kunaiPool.Dequeue();
        gameObject.SetActive(true);
        return gameObject;
    }
}
