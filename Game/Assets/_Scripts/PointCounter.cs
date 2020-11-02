using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour {

    public Text scoreText;
    private int pointCounter = 0;

    private void Awake()
    {
        SetScoreText();
    }

    public void AddValue(int val)
    {
        pointCounter += val;
        SetScoreText();
    }

    public int getCountedValue()
    {
        return pointCounter;
    }

    private void SetScoreText()
    {
        scoreText.text = "Score: " + pointCounter;
    }
}
