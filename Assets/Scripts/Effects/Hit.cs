using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }
    private void AddToPool()
    {
        HitPool.instance.AddToPool(gameObject);
    }
}
