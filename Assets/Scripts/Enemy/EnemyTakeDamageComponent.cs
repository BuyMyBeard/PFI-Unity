using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageComponent : MonoBehaviour
{
    [SerializeField] LayerMask attackerLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Die()
    {
        transform.parent.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
