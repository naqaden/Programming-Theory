using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	//ENCAPSULATION
	private TMP_Text scoreText;
    private int score = 12;
	private int scoreMin = 0;
	private int scoreMax = 40;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
	}

    //ENCAPSULATION
    public void updateScore(int delta)
    {
		score += delta;
        score = Math.Min(score, scoreMax);
        score = Math.Max(score, scoreMin);
		scoreText.text = score.ToString();

        //check if game is over
        if(score == 0)
        {
			SceneManager.LoadScene(2);
		}
        else if(score == 40)
        {
			SceneManager.LoadScene(3);
		}
	}

    public int getScore()
    {
        return score;
    }
}
