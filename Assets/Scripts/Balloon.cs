using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	//ENCAPSULATION
	private AudioSource audioSource;

	private int featherValue = 3;
	private float timeLost = 0;
	private bool isLost = false;
	private float moveSpeed = 3f;

	// Start is called before the first frame update
	void Start()
    {
		//TODO assign data manager
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
			Pop(-featherValue); // ABSTRACTION
		}
		if(other.CompareTag("Projectile"))
		{
			int penalty = (int)Math.Floor(timeLost/2f);
			penalty = Math.Max(featherValue - 1, penalty);
			Pop(featherValue - penalty); // ABSTRACTION
		}
	}

	public void GetLost()
	{
		transform.SetParent(null);
		isLost = true;
	}

	//ENCAPSULATION
	private void Pop(int feathers)
	{
		audioSource.Play();
		GetComponent<Renderer>().enabled = false; //make balloon invisible while sound plays
		Destroy(gameObject, audioSource.clip.length); //delay destruction until sound is complete

		//TODO update data manager feather count
	}
}
