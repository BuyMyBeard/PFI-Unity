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
        if (collision.gameObject.name == "MC")
        {
            score.CollectCoin();
            StartCoroutine(PlaySoundDelay());
        }
    }

    IEnumerator PlaySoundDelay()
    {
        audioSource.volume = 0.01f;
        audioManager.PlaySFX(0);
        yield return new WaitForSeconds(0.8f);
        audioSource.volume = 1f;

        gameObject.SetActive(false);
    }

}
