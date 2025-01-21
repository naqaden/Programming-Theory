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
	private Quaternion nockRotStart = Quaternion.Euler(0,-85,0); //local
	private Vector3 nockPosStart = new Vector3(0.002f,0,-0.0004f); //local
	private Vector3 nockPosEnd = new Vector3(0.000f,0,-0.0002f); //local

	private bool isDrawing = false;
	private float drawTime = 0;
	private float drawTimeMax = 2;
	private float drawSpeed; //calc'd in Start() with delta drawPosStart over drawTimeTotal secs
	private Vector3 drawPosEnd = new Vector3(0.001f,0,-0.0003f); //local

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
			}
			else
			{
				isNocking = false; //so this only happens once
				transform.localPosition = nockPosEnd;
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
				transform.rotation = Quaternion.LookRotation(rb.velocity);
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
	}

	public void shootProjectile()
	{
		isDrawing = false;
		isShot = true;
		transform.SetParent(null); //detach
		transform.position = Camera.main.transform.position + Camera.main.transform.forward;
		rb.isKinematic = false;
		float force = 30f * Math.Min(drawTime, drawTimeMax);
		Vector3 trajectory = Camera.main.transform.forward;
		rb.AddForce(trajectory * force, ForceMode.Impulse);
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
