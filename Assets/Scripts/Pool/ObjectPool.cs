using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    readonly ConcurrentBag<GameObject> pool = new ConcurrentBag<GameObject>();

    public void Add(GameObject obj) => pool.Add(obj);

    public bool TryTake(out GameObject toGive) => pool.TryTake(out toGive);

}
