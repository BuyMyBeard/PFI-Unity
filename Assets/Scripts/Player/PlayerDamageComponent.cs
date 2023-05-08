using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] LayerMask damageLayer;
    PlayerMove playerMove;

    bool isInvulnerable;
    private void Awake()
    {
        launchDirection = launchDirection.normalized;
        playerMove = GetComponent<PlayerMove>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvulnerable && collision.IsTouchingLayers(damageLayer))
        {
            StartCoroutine(TakeKnockback(collision));
            StartCoroutine(BecomeInvincible(invicibilityTime));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isInvulnerable && collision.IsTouchingLayers(damageLayer))
        {
            StartCoroutine(TakeKnockback(collision));
            StartCoroutine(BecomeInvincible(invicibilityTime));
        }
    }

    IEnumerator TakeKnockback(Collider2D collision)
    {
        float deltaX = transform.position.x - collision.transform.position.x;
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
