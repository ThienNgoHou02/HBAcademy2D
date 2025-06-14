using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPool : MonoBehaviour
{
    public static GhostPool instance { get; private set; }

    [Header("Ghost(GameObject)")]
    public GameObject ghostGameObject;

    private Queue<GameObject> ghostPool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
    }
    private void CreatePoolComponent()
    {
        GameObject gameObject = Instantiate(ghostGameObject);
        gameObject.transform.parent = transform;
        AddToPool(gameObject);
    }
    public void AddToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        ghostPool.Enqueue(gameObject);
    }
    public GameObject GetFromPool()
    {
        if (ghostPool.Count <= 0)
            CreatePoolComponent();

        GameObject gameObject = ghostPool.Dequeue();
        gameObject.SetActive(true);
        return gameObject;
    }
}
