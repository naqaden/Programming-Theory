using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class BalloonAnimal : Balloon //INHERITANCE
{
	protected override int scoreValue => 6; //POLYMORPHISM

	// Update is called once per frame
	protected override void Update() //POLYMORPHISM
	{
		transform.Rotate(0, 0, 90 * Time.deltaTime); //spin the snake
		base.Update(); //INHERITANCE
	}
}
