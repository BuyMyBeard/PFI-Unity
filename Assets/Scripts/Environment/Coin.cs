using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Score score;
    private void Awake()
    {
        score = FindObjectOfType<Score>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        score.CollectCoin();
        gameObject.SetActive(false);
    }
}
