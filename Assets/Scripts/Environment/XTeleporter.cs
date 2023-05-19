using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class XTeleporter : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] LayerMask canTeleport;
    [SerializeField] float offset;

    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((canTeleport.value & (1 << collision.gameObject.layer)) == 0) return;
        Transform objToDisplace = collision.transform;
        float xdisp = destination.transform.position.x - transform.position.x;
        if (xdisp > 0)
        {
            xdisp += -boxCollider.size.x - offset;
        } else if (xdisp < 0)
        {
            xdisp += boxCollider.size.x + offset;
        }
        // if it happens to be exactly 0 then there is no offset
        
        objToDisplace.Translate(new Vector3(xdisp, 0, 0));

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
