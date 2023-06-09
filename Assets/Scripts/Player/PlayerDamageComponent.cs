using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerDamageComponent : MonoBehaviour
{
    SpriteRenderer sprite;
    [SerializeField] float invicibilityTime = 3;
    [SerializeField] float stunnedTime = 2;
    [SerializeField] Vector2 launchDirection;
    [SerializeField] float launchSpeed;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] LayerMask damageLayer;
    [SerializeField] LayerMask lavaLayer;
    PlayerMove playerMove;
    private AudioManager audioManager;

    bool isInvulnerable = false;
    private void Awake()
    {
        launchDirection = launchDirection.normalized;
        playerMove = GetComponent<PlayerMove>();
        sprite = GetComponent<SpriteRenderer>();
        audioManager = GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((lavaLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            playerMove.Die();
        }

        if (!isInvulnerable && (damageLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            StartCoroutine(TakeKnockback(collision.transform));
            StartCoroutine(BecomeInvincible(invicibilityTime));
        }
        if (!playerMove.stunned && playerMove.IsFalling && (attackLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            EnemyTakeDamageComponent takeDamage = collision.gameObject.GetComponent<EnemyTakeDamageComponent>();
            if (takeDamage != null)
            {
                takeDamage.Die();
                playerMove.bouncedOnEnemy = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isInvulnerable && (damageLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            StartCoroutine(TakeKnockback(collision.transform));
            StartCoroutine(BecomeInvincible(invicibilityTime));
        }
    }


    IEnumerator TakeKnockback(Transform colliderPosition)
    {
        float deltaX = transform.position.x - colliderPosition.position.x;
        Vector2 launchVector = launchDirection * launchSpeed;
        if (deltaX < 0 || (deltaX == 0 && Random.Range(0, 2) == 0))
            launchVector.x = -launchVector.x;

        playerMove.TakeKnockBack(launchVector);
        yield return new WaitForSeconds(stunnedTime);
        playerMove.Recover();
    }

    public IEnumerator BecomeInvincible(float time)
    {
        isInvulnerable = true;
        Color color = sprite.color;
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(time);
        color.a = 1;
        sprite.color = color;
        isInvulnerable = false;
    }
    public void TriggerInvincibility(float time)
    {
        StartCoroutine(BecomeInvincible(time));
    }

}
