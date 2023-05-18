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
    private AudioManager audioManager;

    //AudioManagerComponent sfx;
    AudioSource audioSource;
    Animations currentAnimation = Animations.Idle;

    PlayerInputComponent inputs;
    public bool stunned = false;
    float coyoteTimeElapsed = 0;

    public bool bouncedOnEnemy = false;
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
    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }
    new private void FixedUpdate()
    {
        if (stunned)
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
        if (isGrounded)
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
        if ((inputs.JumpPressInput && (isGrounded || IsCoyoteTime) && !IsJumping) || bouncedOnEnemy)
        {
            JumpNoise();
            newVelocity.y = jumpVelocity;
            bouncedOnEnemy = false;
        }
    }

    protected override void AddGravity()
    {
        if (isGrounded)
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
    void JumpNoise()
    {
        audioSource.volume = 0.01f;
        StartCoroutine(PlaySoundWithDelay());
    }

    IEnumerator PlaySoundWithDelay()
    {
        audioManager.PlaySFX(0);
        yield return new WaitForSeconds(0.5f);
        audioSource.volume = 1f;
    }

}