using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileHoly : Projectile //INHERITANCE
{
	protected override int drawCost => 2; //POLYMORPHISM
	protected override float drawTimeMax => 1; //POLYMORPHISM
	protected override float speed => 60; //POLYMORPHISM

	private void Awake()
	{
		isNocking = false;
		transform.localPosition = nockPosEnd;
	}

	public override void shootProjectile() //POLYMORPHISM
	{
		base.shootProjectile(); //INHERITANCE
		rb.useGravity = false;
	}
}
