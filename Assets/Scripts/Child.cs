using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
	// ENCAPSULATION
	private float moveDistance = 10f;
	private float maxDistanceFromOrigin = 20f;
	//private float minDistanceFromOrigin = 2f;
	public float moveSpeed = 2f;
	public float pauseDuration = 3f;

	private Vector3 targetPosition;
	private float timeSinceLastMove = 0f;
	private bool isMoving = false;
	private bool needsBalloon = false;

	public GameObject balloon;
	public float loseChance = 0.2f;

	// Start is called before the first frame update
	void Start()
	{
		//every child is special
		moveSpeed += Random.Range(-0.5f, 0.5f);
		pauseDuration += Random.Range(-2f, 0f);
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
				else if(transform.childCount > 0 && Random.value < loseChance)
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

		// Set the target position 10 units away in a random direction
		float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
		Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
		targetPosition = transform.position + randomDirection * moveDistance;

		// Check if the new targetPosition is beyond the max distance from origin
		if(Vector3.Distance(targetPosition, Vector3.zero) > maxDistanceFromOrigin)
		{
			Vector3 towardOrigin = new Vector3(-transform.position.x, 0, -transform.position.z);
			targetPosition = transform.position + towardOrigin.normalized * moveDistance;
			if(transform.childCount == 0)
			{
				needsBalloon = true;
			}
		}

		//TODO: prevent children from walking too close to or through player
	}

	// ENCAPSULATION
	private void AcquireBalloon()
	{
		needsBalloon = false;

		if(transform.childCount == 0)
		{
			Instantiate(balloon, transform.position + Vector3.up * 2, Quaternion.identity, transform);
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
			heldBalloonScript.GetLost();
		}
		else
		{
			Debug.Log("child has no balloon");
		}
	}
}
