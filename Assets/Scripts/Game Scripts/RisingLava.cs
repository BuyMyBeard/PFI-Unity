using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{
    [SerializeField] float speed = 5;
    private Vector2 direction = Vector2.up;
    void Awake()
    {
        direction = direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }
}
