using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Score score;
    private AudioManager audioManager;
    AudioSource audioSource;
    private void Awake()
    {
        score = FindObjectOfType<Score>();
        audioSource = GetComponent<AudioSource>();
        audioManager = GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        score.CollectCoin();
        audioManager.PlaySFX(0);
        new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
