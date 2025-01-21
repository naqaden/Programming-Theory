using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	//ENCAPSULATION
	private TMP_Text scoreText;
    private int score = 12;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    //ENCAPSULATION
    public void updateScore(int delta)
    {
		score += delta;
		scoreText.text = score.ToString();
	}
}
