using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.UI.Image;

public class Child : MonoBehaviour
{
	// ENCAPSULATION
	private AudioSource audioHappy;
	private AudioSource audioSad;

	public GameObject balloon;
	public GameObject balloonRare;
	private Quaternion balloonRareRot = Quaternion.Euler(-90,0,0);
	private float rareBalloonChance = 1.2f;
	private float loseBalloonChance = 1.2f;

	private float moveDistance = 10f;
	private float maxDistanceFromOrigin = 20f;
	private float minDistanceFromOrigin = 2f;
	private float moveSpeed = 3f;
	private float pauseDuration = 2f;

	private Vector3 targetPosition;
	private float timeSinceLastMove = 0f;
	private bool isMoving = false;
	private bool needsBalloon = true;

	// Start is called before the first frame update
	void Start()
	{
		//every child is special
		moveSpeed += Random.Range(-0.5f, 0.5f);
		pauseDuration += Random.Range(-1f, 0f);

		AudioSource[] audioSources = GetComponents<AudioSource>();
		audioHappy = audioSources[0];
		audioSad = audioSources[1];
	}

	// Update is called once per frame
	void Update()
	{
		if(isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			if(Vector3.Distance(transform.position, targetPosition) < 0.5f)
			{
				isMoving = false;
				timeSinceLastMove = 0f;

				if(needsBalloon)
				{
					AcquireBalloon(); // ABSTRACTION
				}
				else if(transform.childCount > 0 && Random.value <= loseBalloonChance)
				{
					LoseBalloon(); // ABSTRACTION
				}
			}
		}
		else
		{
			timeSinceLastMove += Time.deltaTime;
			if(timeSinceLastMove >= pauseDuration)
			{
				StartWandering(); // ABSTRACTION
			}
		}
	}

	// ENCAPSULATION
	private void StartWandering()
	{
		isMoving = true;
		float randomAngle;
		Vector3 randomDirection;

		do
		{
			// Set the target position 10 units away in a random direction
			randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
			randomDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
			targetPosition = transform.position + randomDirection * moveDistance;
		} while(willClipPlayer()); //ABSTRACTION

		// Check if the new targetPosition is beyond the max distance from origin
		if(Vector3.Distance(targetPosition, Vector3.zero) > maxDistanceFromOrigin)
		{
			Vector3 towardOrigin = new Vector3(-transform.position.x, 0, -transform.position.z);
			targetPosition = transform.position + towardOrigin.normalized * (moveDistance - minDistanceFromOrigin);
			if(transform.childCount == 0)
			{
				needsBalloon = true;
			}
		}

		//TODO: prevent children from walking too close to or through player
	}

	// ENCAPSULATION
	private bool willClipPlayer()
	{
		if(Vector3.Distance(transform.position, Vector3.zero) <= minDistanceFromOrigin)
		{
			Debug.Log("child got too close!");
			return false; //already too close, get away
		}

		//calcs line between curpos and destination
		//calcs nearest point on line to player
		//returns if nearest point is too close
		Vector3 deltaToDest = targetPosition - transform.position;
		Vector3 deltaToPlayer = Vector3.zero - transform.position;
		float projection = Vector3.Dot(deltaToPlayer, deltaToDest) / Vector3.Dot(deltaToDest, deltaToDest);
		projection = Mathf.Clamp01(projection);
		Vector3 nearestToPlayer = transform.position + projection * deltaToDest;
		return Vector3.Distance(nearestToPlayer, Vector3.zero) <= minDistanceFromOrigin;
		//return Vector3.Distance(targetPosition, Vector3.zero) <= minDistanceFromOrigin;
	}

	// ENCAPSULATION
	private void AcquireBalloon()
	{
		needsBalloon = false;

		if(transform.childCount == 0)
		{
			if(Random.value <= rareBalloonChance)
			{
				Instantiate(balloonRare, transform.position + Vector3.up * 2, balloonRareRot, transform);
			}
			else
			{
				Instantiate(balloon, transform.position + Vector3.up * 2, Quaternion.identity, transform);
			}
			audioHappy.Play();
		}
		else
		{
			Debug.Log("child already has balloon");
		}
	}

	// ENCAPSULATION
	private void LoseBalloon()
	{
		if(transform.childCount > 0)
		{
			Transform heldBalloon = transform.GetChild(0);
			Balloon heldBalloonScript = heldBalloon.GetComponent<Balloon>();
			heldBalloonScript.Release();
			audioSad.Play();
		}
		else
		{
			Debug.Log("child has no balloon");
		}
	}
}
