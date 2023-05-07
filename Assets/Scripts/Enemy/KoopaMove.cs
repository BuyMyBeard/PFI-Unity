using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaMove : GroundedCharacter
{
    bool isMovingRight = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new private void FixedUpdate()
    {
        newVelocity = Velocity;
        FloorCheck();
        WallCheck();
        SetHorizontalVelocity();
        AddGravity();
        AddSlopeCompensation();
        LimitVelocity();
        Velocity = newVelocity;
    }

    private void SetHorizontalVelocity()
    {
        newVelocity.x = horizontalSpeed * (isMovingRight ? 1 : -1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

        }
    }

    private void WallCheck()
    {
        
    }
}
