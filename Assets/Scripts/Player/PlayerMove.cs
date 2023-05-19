using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputComponent))]
public class PlayerMove : GroundedCharacter
{
    enum Animations { MCStunned, MCIdle, MCRun, MCRaising, MCFalling };
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
    Animations currentAnimation = Animations.MCIdle;

    PlayerInputComponent inputs;
    public bool stunned = false;
    float coyoteTimeElapsed = 0;

    public bool bouncedOnEnemy = false;
    private bool isJumpSoundPlaying = false;

    public bool IsCoyoteTime
    {
        get => coyoteTimeElapsed < coyoteTime;
    }

    new private void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        inputs = GetComponent<PlayerInputComponent>();
        animator = GetComponent<Animator>();
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

    private void Update()
    {
        if (stunned)
            return;
        if (isGrounded)
        {
            if (Velocity.x != 0)
                SetAnimation(Animations.MCRun);
            else
                SetAnimation(Animations.MCIdle);
        }
        else
        {
            if (Velocity.y <= 0)
                SetAnimation(Animations.MCFalling);
            else
                SetAnimation(Animations.MCRaising);
        }

        if (IsJumping && !isJumpSoundPlaying)
        {
            StartCoroutine(PlayJumpSoundDelay());
        }
    }

    private void SetAnimation(Animations animation)
    {
        if (currentAnimation != animation)
        {
            if (animation == Animations.MCRun)
                audioSource.Play();
            else
                audioSource.Stop();

            animator.Play(animation.ToString());
            currentAnimation = animation;
        }
    }


    private void SetHorizontalVelocity()
    {
        newVelocity.x = inputs.MoveInput.x * horizontalSpeed;
        if (inputs.MoveInput.x != 0)
            Sprite.flipX = inputs.MoveInput.x < 0;
    }
    private void CheckInputs()
    {
        if ((inputs.JumpPressInput && (isGrounded || IsCoyoteTime) && !IsJumping) || bouncedOnEnemy)
        {
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
        SetAnimation(Animations.MCStunned);
        currentAnimation = Animations.MCStunned;
        RB.velocity = knockback;
        RB.gravityScale = stunnedGravityScale;
        stunned = true;
        RB.sharedMaterial = ragdollPhysics;
        //sfx.PlaySFX(0);
    }
    public void Recover()
    {
        SetAnimation(Animations.MCIdle);
        RB.gravityScale = 0;
        stunned = false;
    }
    IEnumerator PlayJumpSoundDelay()
    {
        isJumpSoundPlaying = true;
        audioManager.PlaySFX(0);
        yield return new WaitForSeconds(1f);
        isJumpSoundPlaying = false;
    }
}