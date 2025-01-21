using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class Balloon : MonoBehaviour
{
	//ENCAPSULATION
	private Score score;
	private AudioSource audioSource;

	private int scoreValue = 3;
	private float timeLost = 0;
	private bool isLost = false;
	private float moveSpeed = 3f;

	// Start is called before the first frame update
	void Start()
	{
		score = EventSystem.current.GetComponent<Score>();
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		if(isLost)
		{
			timeLost += Time.deltaTime;
			Vector3 targetPosition = transform.position + Vector3.up * 2;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Sky"))
		{
			Pop(-scoreValue); // ABSTRACTION
		}
		if(other.CompareTag("Projectile") && other.transform.parent == null) //exclude children who wander into player
		{
			if(transform.parent == null) //balloon is lost
			{
				int penalty = (int)Math.Floor(timeLost/2f);
				penalty = Math.Min(scoreValue - 1, penalty);
				Pop(scoreValue - penalty); // ABSTRACTION
			}
			else //child was still holding. maximum penalty
			{
				Pop(-scoreValue - 1); // ABSTRACTION
			}
		}
	}

	public void GetLost()
	{
		transform.SetParent(null);
		isLost = true;
	}

	//ENCAPSULATION
	private void Pop(int delta)
	{
		score.updateScore(delta);
		audioSource.Play();
		GetComponent<Renderer>().enabled = false; //make balloon invisible while sound plays
		Destroy(gameObject, audioSource.clip.length); //delay destruction until sound is complete
	}
}
