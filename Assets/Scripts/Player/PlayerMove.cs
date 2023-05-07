using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputComponent))]
public class PlayerMove : GroundedCharacter
{
    enum Animations { Stunned, Idle, Running, Jumping, Launch, Raising, Falling, Land };
    [Space(20)]
    [Header("Player")]
    [Space(10)]
    [SerializeField] float ascendingDrag = 1;
    [SerializeField] float holdingJumpDrag = 1;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float stunnedGravityScale = 1;
    [SerializeField] PhysicsMaterial2D ragdollPhysics;
    [SerializeField] float dropPlatformLength = 0.5f;

    //AudioManagerComponent sfx;
    AudioSource audioSource;
    Animations currentAnimation = Animations.Idle;

    PlayerInputComponent inputs;
    bool isDroppingPlatform = false, stunned = false;
    bool ended = false;
    float coyoteTimeElapsed = 0;

    public bool IsCoyoteTime
    {
        get => coyoteTimeElapsed < coyoteTime;
    }

    new private void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        inputs = GetComponent<PlayerInputComponent>();
        //sfx = GetComponent<AudioManagerComponent>();
    }

    new private void FixedUpdate()
    {
        if (stunned || ended)
            return;

        newVelocity = Velocity;
        FloorCheck();
        SetHorizontalVelocity();
        AddGravity();
        AddSlopeCompensation();
        CheckInputs();
        AddDrag();
        LimitVelocity();
        Velocity = newVelocity;
        if (IsGrounded)
            ResetCoyoteTime();
    }

    private void SetAnimation(Animations animation)
    {
        //if (currentAnimation != animation)
        //{
        //    if (animation == Animations.Running)
        //        audioSource.Play();
        //    else
        //        audioSource.Stop();

        //    animator.Play(dictAnimations[animation]);
        //    currentAnimation = animation;
        //}
    }


    private void SetHorizontalVelocity()
    {
        newVelocity.x = inputs.MoveInput.x * horizontalSpeed;
        if (inputs.MoveInput.x != 0)
            Sprite.flipX = inputs.MoveInput.x > 0;
    }

    private void CheckInputs()
    {
        if (inputs.JumpPressInput && (IsGrounded || IsCoyoteTime) && !IsJumping)
            newVelocity.y = jumpVelocity;

    }

    protected override void AddGravity()
    {
        if (inputs.DropInput && isTouchingPlatform && !isDroppingPlatform)
        {
            StartCoroutine(DropPlatform());
        }
        if (isDroppingPlatform)
            isTouchingPlatform = false;
        if (IsGrounded)
        {
            newVelocity.y = 0;
            RB.sharedMaterial = highFriction;
        }
        else
        {
            newVelocity.y += gravAcceleration * Time.deltaTime;
            coyoteTimeElapsed += Time.deltaTime;
            RB.sharedMaterial = noFriction;
        }
    }

    IEnumerator DropPlatform()
    {
        coyoteTimeElapsed += coyoteTime;
        newVelocity.y = 0;
        isDroppingPlatform = true;
        yield return new WaitForSeconds(dropPlatformLength);
        isDroppingPlatform = false;
    }

    private void AddDrag()
    {
        if (IsJumping)
        {
            newVelocity.y += ascendingDrag * Time.deltaTime;
            if (inputs.JumpHoldInput)
                newVelocity.y += holdingJumpDrag * Time.deltaTime;
        }
    }

    public void ResetCoyoteTime()
    {
        coyoteTimeElapsed = 0;
    }
    public void TakeKnockBack(Vector2 knockback)
    {
        SetAnimation(Animations.Stunned);
        currentAnimation = Animations.Stunned;
        RB.velocity = knockback;
        RB.gravityScale = stunnedGravityScale;
        stunned = true;
        RB.sharedMaterial = ragdollPhysics;
        //sfx.PlaySFX(0);
    }
    public void Recover()
    {
        currentAnimation = Animations.Idle;
        SetAnimation(Animations.Idle);
        RB.gravityScale = 0;
        stunned = false;
    }
}