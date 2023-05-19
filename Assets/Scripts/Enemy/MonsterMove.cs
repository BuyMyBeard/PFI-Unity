using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : GroundedCharacter
{
    [SerializeField] float ledgeCheckX;
    [SerializeField] float ledgeCheckY;
    [SerializeField] float ledgeCheckDistance;
    [SerializeField] float wallCheckDistance = 1f;
    [SerializeField] bool IsFlipped = false;

    float MovingBackMult => IsFlipped ? 1 : -1;

    void Flip()
    {
        IsFlipped = !IsFlipped;
        Sprite.flipX = !Sprite.flipX;
        CC.offset *= -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsFlipped)
        {
            Sprite.flipX = !Sprite.flipX;
            CC.offset *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new private void FixedUpdate()
    {
        newVelocity = Velocity;
        FloorCheck();
        LedgeCheck();
        WallCheck();
        SetHorizontalVelocity();
        AddGravity();
        AddSlopeCompensation();
        LimitVelocity();
        Velocity = newVelocity;
    }

    private void SetHorizontalVelocity()
    {
        newVelocity.x = horizontalSpeed * MovingBackMult;
    }

    private void LedgeCheck()
    {
        Vector2 ledgeCheckDir = new Vector2(ledgeCheckX * MovingBackMult, ledgeCheckY).normalized;
        RaycastHit2D isLedge = Physics2D.Raycast(transform.position, ledgeCheckDir, ledgeCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, ledgeCheckDir * ledgeCheckDistance, Color.magenta);

        if (!isLedge && isGrounded)
        {
            Flip();
        }
    }

    private void WallCheck()
    {
        Vector2 wallCheckDir = new Vector2(MovingBackMult, 0);
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, wallCheckDir, wallCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, wallCheckDir* wallCheckDistance, Color.yellow);

        if (wallHit)
        {
            Flip();
        }
    }
}
