using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileHoly : Projectile // INHERITANCE
{
	// POLYMORPHISM
	protected override bool isNocking => false;
	protected override int drawCost => 2;
	protected override float drawTimeMax => 0.5f;
	protected override float speed => 60;
	
	// POLYMORPHISM
	protected override void Awake()
	{
		base.Awake(); // INHERITANCE
		transform.localPosition = nockPosEnd;
	}

	// POLYMORPHISM
	public override void shootProjectile()
	{
		base.shootProjectile(); // INHERITANCE
		rb.useGravity = false;
	}
}
