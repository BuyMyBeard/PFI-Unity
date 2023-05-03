using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    public ObjectPoolComponent AssociatedPool { get; set; }
}
