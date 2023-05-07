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
        SetHorizontalVelocity();
        AddGravity();
        AddSlopeCompensation();
        LimitVelocity();
        Velocity = newVelocity;
    }

    private void SetHorizontalVelocity()
    {
        newVelocity.x = horizontalSpeed * ;
    }
}
