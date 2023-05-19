using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Coin : MonoBehaviour
{
    Score score;
    private AudioManager audioManager;
    private void Awake()
    {
        score = FindObjectOfType<Score>();
        audioManager = GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "MC")
        {
            score.CollectCoin();
            audioManager.PlaySFX(0);
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
