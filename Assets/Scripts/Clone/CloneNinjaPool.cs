using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneNinjaPool : MonoBehaviour
{
    public static CloneNinjaPool instance {  get; private set; }

    [Header("Clone Ninja(GameObject)")]
    public GameObject cloneNinjaGameObject;
    
    private Queue<GameObject> cloneNinjaPool = new Queue<GameObject>();
    private void Awake()
    {
        instance = this;
    }
    private void CreatePoolComponent()
    {
        GameObject gameObject = Instantiate(cloneNinjaGameObject);
        gameObject.transform.parent = transform;
        AddToPool(gameObject);
    }
    public void AddToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        cloneNinjaPool.Enqueue(gameObject);
    }
    public GameObject GetFromPool()
    {
        if (cloneNinjaPool.Count <= 0)
            CreatePoolComponent();

        GameObject gameObject = cloneNinjaPool.Dequeue();
        gameObject.SetActive(true);
        return gameObject;
    }
}
