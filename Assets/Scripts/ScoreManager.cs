using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public static int scoreCount;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text is not assigned in the inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Notes : " + Mathf.Round(scoreCount);
        }
    }

    public void AddScore(int points)
    {
        scoreCount += points;
    }
}