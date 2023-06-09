using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputComponent))]
public class PlayerMove : GroundedCharacter
{
    enum Animations { MCStunned, MCIdle, MCRun, MCRaising, MCFalling, MCDying };
    [Space(20)]
    [Header("Player")]
    [Space(10)]
    [SerializeField] float ascendingDrag = 1;
    [SerializeField] float holdingJumpDrag = 1;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float stunnedGravityScale = 1;
    [SerializeField] float deadSpeed = 1;
    [SerializeField] float timeBeforeDeath = 4;
    [SerializeField] PhysicsMaterial2D ragdollPhysics;
    private AudioManager audioManager;

    //AudioManagerComponent sfx;
    AudioSource audioSource;
    Animations currentAnimation = Animations.MCIdle;

    PlayerInputComponent inputs;
    public bool stunned = false;
    float coyoteTimeElapsed = 0;

    public bool bouncedOnEnemy = false;
    private bool isDead = false;
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
        if (isDead)
        {
            transform.Translate(deadSpeed * Time.fixedDeltaTime * Vector2.down);
            return;
        }
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
        if (stunned || isDead)
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
    }

    private void SetAnimation(Animations animation)
    {
        if (currentAnimation != animation)
        {
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
            audioManager.PlaySFX(0);
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
        audioManager.PlaySFX(2);
    }
    public void Recover()
    {
        SetAnimation(Animations.MCIdle);
        RB.gravityScale = 0;
        stunned = false;
    }

    internal void Die()
    {
        audioManager.PlaySFX(3);
        inputs.enabled = false;
        isDead = true;
        RB.velocity = Vector2.zero;
        CC.enabled = false;
        RB.gravityScale = 0;
        SetAnimation(Animations.MCDying);
        StartCoroutine(WaitForDeath());
    }
    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(timeBeforeDeath);
        SceneManager.LoadScene("Main Menu");
    }
}