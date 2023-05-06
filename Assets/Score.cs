using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] int score = 0;
    private void Awake()
    {
        scoreDisplay.SetText(score.ToString());
    }
    public void CollectCoin()
    {
        score++;
        scoreDisplay.SetText(score.ToString());
    }
} 
