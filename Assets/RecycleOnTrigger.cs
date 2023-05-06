using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleOnTrigger : MonoBehaviour, IPoolable
{
    
    [SerializeField] LayerMask ProjectileBoundariesLayer;

    public ObjectPoolComponent AssociatedPool { get; set; }

    private void OnTriggeEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(ProjectileBoundariesLayer))
        {
            AssociatedPool.ReturnElement(gameObject);
        }
    }
}
