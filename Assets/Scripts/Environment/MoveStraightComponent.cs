using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraightComponent : MonoBehaviour
{
    public Vector2 direction = Vector2.down;
    [SerializeField] float speed = 10;
    void Awake()
    {
        direction = direction.normalized;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }
}
