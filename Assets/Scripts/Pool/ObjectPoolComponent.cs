using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolComponent : MonoBehaviour
{
    ObjectPool pool = new();
    [SerializeField] GameObject poolable;
    [SerializeField] int poolSize = 25;

    // Start is called before the first frame update
    void Start()
    {
        pool = new();
        FillPool();
        transform.parent = null;
    }
    public GameObject GetElement()
    {
        GameObject obj;
        if (!pool.TryTake(out obj))
        {
            obj = Instantiate(obj);
            foreach (IPoolable component in obj.GetComponents<IPoolable>())
                component.AssociatedPool = this;
        
        }
        return obj;
    }

    private void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject clone = Instantiate(poolable, transform);
            clone.SetActive(false);
            foreach (IPoolable component in clone.GetComponents<IPoolable>())
                component.AssociatedPool = this;
            pool.Add(clone);
        }
    }
    public void ReturnElement(GameObject obj)
    {
        obj.SetActive(false);
        pool.Add(obj);
    }
}
