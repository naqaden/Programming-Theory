
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Projectile:MonoBehaviour
{
	// ENCAPSULATION
	protected Rigidbody rb;

	protected virtual bool isNocking { get; set; } = true;
	private float nockTime = 0;
	private float nockTimeTotal = 1;
	private float nockSpeed; //calc'd in Start() with delta nockPosStart over nockTimeTotal secs
	private Quaternion nockRotStart = Quaternion.Euler(0,-85,0); //local
	private Vector3 nockPosStart = new Vector3(0.002f,0,-0.0004f); //local
	protected Vector3 nockPosEnd = new Vector3(0.000f,0,-0.0002f); //local

	private Score score;
	private bool isDrawing = false;
	protected virtual int drawCost => 1;
	private float drawTime = 0;
	protected virtual float drawTimeMax => 1;
	private float drawSpeed; //calc'd in Start() with delta drawPosStart over drawTimeTotal secs
	private Vector3 drawPosEnd = new Vector3(0.001f,0,-0.0003f); //local

	private bool isShot = false;
	protected virtual float speed => 50;
	private float airTime = 0;
	private float airTimeMax = 15;
	private bool isGrounded = false;
	
	protected virtual void Awake()
	{
		score = EventSystem.current.GetComponent<Score>();
		rb = gameObject.GetComponent<Rigidbody>();
		nockSpeed = nockPosStart.magnitude / nockTimeTotal;
		drawSpeed = drawPosEnd.magnitude / drawTimeMax;

		if(isNocking) //excludes ProjectileHoly since it starts nocked
		{
			transform.localPosition = nockPosStart;
		}
		transform.localRotation = nockRotStart;
	}

	// Update is called once per frame
	protected void Update()
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

	// ENCAPSULATION
	public int getDrawCost()
	{
		return drawCost;
	}

	// ENCAPSULATION
	public Vector3 getNockPosEnd()
	{
		return nockPosEnd;
	}

	public void drawProjectile()
	{
		if(isNocking == false && score.getScore() >= drawCost)
		{
			isDrawing = true;
			drawTime = 0;
		}
	}

	public virtual void shootProjectile()
	{
		isDrawing = false;
		isShot = true;
		transform.SetParent(null); //detach
		transform.position = Camera.main.transform.position + Camera.main.transform.forward;
		rb.useGravity = true;
		float force = Math.Min(drawTime, drawTimeMax) * speed;
		Vector3 trajectory = Camera.main.transform.forward;
		rb.AddForce(trajectory * force, ForceMode.Impulse);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Terrain"))
		{
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			isGrounded = true;
		}
	}
}
