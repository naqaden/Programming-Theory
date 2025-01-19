using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
	// ENCAPSULATION
	private Rigidbody rb;

	private bool isNocking = true;
	private float nockTime = 0;
	private float nockTimeTotal = 1;
	private float nockSpeed; //calc'd in Start() with delta nockPosStart over nockTimeTotal secs
	private Vector3 nockPosStart = new Vector3(0.002f,0,0); //local
	private Quaternion nockRotStart = Quaternion.Euler(0,0,-90); //local
	private Vector3 nockPosEnd = Vector3.zero; //local

	private bool isDrawing = false;
	private float drawTime = 0;
	private float drawTimeMax = 2;
	private float drawSpeed; //calc'd in Start() with delta drawPosStart over drawTimeTotal secs
	private Vector3 drawPosStart = Vector3.zero;  //local
	private Vector3 drawPosEnd = new Vector3(0.001f,0,0); //local

	private bool isShot = false;
	private float airTime = 0;
	private float airTimeMax = 15;
	private bool isGrounded = false;

	// Start is called before the first frame update
	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		nockSpeed = nockPosStart.magnitude / nockTimeTotal;
		drawSpeed = drawPosEnd.magnitude / drawTimeMax;

		transform.localPosition = nockPosStart;
		transform.localRotation = nockRotStart;
	}

	// Update is called once per frame
	void Update()
	{
		if(isNocking)
		{
			nockTime += Time.deltaTime;
			if(nockTime < nockTimeTotal)
			{
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, nockPosEnd, nockSpeed * Time.deltaTime);
//Debug.Log("still nocking: " + nockTime + " of 1.0, " + transform.localPosition.x.ToString("F6"));
			}
			else
			{
				isNocking = false; //so this only happens once
				transform.localPosition = nockPosEnd;
//Debug.Log("done nocking: " + nockTime + " of 1.0, " + transform.localPosition.x.ToString("F6"));
			}
		}
		else if(isDrawing)
		{
			drawTime += Time.deltaTime;
			if(drawTime < drawTimeMax)
			{
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, drawPosEnd, drawSpeed * Time.deltaTime);
			}
			else if(transform.localPosition != drawPosEnd) //so this only happens once, if ever
			{
				transform.localPosition = drawPosEnd;
			}
		}
		else if(isShot)
		{
			airTime += Time.deltaTime;
			if(airTime > airTimeMax)
			{
				Destroy(gameObject);
			}

			if(!isGrounded && rb.velocity != Vector3.zero) //velocity check silences LookRotation()'s unsolicited debug spam
			{
				transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(-90, 0, 0);
			}
		}
	}

	// ENCAPSULATION
	public bool getNocking()
	{
		return isNocking;
	}

	// ENCAPSULATION
	public bool getDrawing()
	{
		return isDrawing;
	}

	public void drawProjectile()
	{
		if(isNocking == false)
		{
			isDrawing = true;
			drawTime = 0;
			//slowly move back
		}
		else Debug.Log("still nocking: " + nockTime + " of 1.0");
	}

	public void shootProjectile()
	{
		isDrawing = false;
		isShot = true;
		float force = 50f * Math.Min(drawTime, drawTimeMax);
		transform.SetParent(null); //detach
		// Elven Long Bow arrow's "forward" is -y (down)
		rb.isKinematic = false;
		rb.AddForce(-transform.up * force, ForceMode.Impulse);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Terrain"))
		{
			rb.isKinematic = true;
			isGrounded = true;
		}
	}
}
